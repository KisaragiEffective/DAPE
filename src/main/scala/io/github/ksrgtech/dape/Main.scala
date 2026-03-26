package io.github.ksrgtech.dape

import cats.effect.{ExitCode, IO, Resource}
import cats.implicits.*
import com.comcast.ip4s.*
import com.monovore.decline.Opts
import com.monovore.decline.effect.CommandIOApp
import neotypes.{AsyncDriver, GraphDatabase}
import neotypes.cats.effect.implicits.*
import org.http4s.HttpRoutes
import org.http4s.dsl.io.*
import org.http4s.ember.server.EmberServerBuilder
import org.neo4j.driver.AuthTokens
import io.github.ksrgtech.dape.commandline.Options
import io.github.ksrgtech.dape.config.RootConfig
import io.github.ksrgtech.dape.user.database.UserRepositoryImpl
import io.github.ksrgtech.dape.user.model.{LocalRegisteredUser, UserId}
import io.github.ksrgtech.dape.user.service.{PasswordGenerator, PasswordHasher}

import java.nio.file.{Files, Paths}
import java.util.UUID

object Main
    extends CommandIOApp(
      name = "dape",
      header = "DAPE - Discrete ActivityPub Environment"
    ) {

  override def main: Opts[IO[ExitCode]] =
    Options.opts.map { opts =>
      runApp(opts)
    }

  private def runApp(opts: Options): IO[ExitCode] = {
    val configPath = Paths.get(opts.runDir, "config.json")

    for {
      _ <- IO.whenA(!Files.exists(configPath)) {
        IO.raiseError(new IllegalArgumentException(s"Path ${configPath} does not exist"))
      }
      configJson <- IO.blocking(Files.readString(configPath))
      config     <- RootConfig.deserializeFromJson[IO](configJson)
      _          <- IO.whenA(config.database == null) {
        IO.raiseError(new IllegalArgumentException("database is null"))
      }
      result <- createDriverResource(config).use { driver =>
        runServer(driver)
      }
    } yield result
  }

  private def createDriverResource(config: RootConfig): Resource[IO, AsyncDriver[IO]] =
    GraphDatabase.asyncDriver[IO](
      s"bolt://${config.database.host}:${config.database.port}",
      AuthTokens.basic(config.database.user, config.database.password)
    )

  private def runServer(driver: AsyncDriver[IO]): IO[ExitCode] = {
    val repo = new UserRepositoryImpl[IO](driver)

    val routes = HttpRoutes.of[IO] { case GET -> Root / "init" =>
      // TODO: Implement initialization endpoint
      Ok("TODO")
    }

    val initializeRootUser: IO[Unit] =
      repo.hasRootUser.flatMap { isInitialized =>
        if isInitialized then {
          IO.println("Initialized")
        } else {
          for {
            _              <- IO.println("Not initialized")
            raw            <- PasswordGenerator.generate[IO]()
            _              <- IO.println(s"Your root password: $raw")
            hashedPassword <- PasswordHasher.createHashedPassword[IO](raw)
            userId = UserId(UUID.randomUUID())
            user   = LocalRegisteredUser(userId, "root", hashedPassword)
            _ <- repo.createRootUser(user)
          } yield ()
        }
      }

    for {
      _ <- initializeRootUser.start
      _ <- EmberServerBuilder
        .default[IO]
        .withHost(ipv4"127.0.0.1")
        .withPort(port"0")
        .withHttpApp(routes.orNotFound)
        .build
        .useForever
    } yield ExitCode.Success
  }
}
