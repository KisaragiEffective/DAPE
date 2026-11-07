package io.github.ksrgtech.dape.user.database

import cats.effect.kernel.Async
import cats.implicits.*
import neotypes.AsyncDriver
import neotypes.mappers.ResultMapper
import neotypes.syntax.all.*
import io.github.ksrgtech.dape.user.model.{LocalRegisteredUser, User}
import io.github.ksrgtech.dape.base.cats.assert

final class UserRepositoryImpl[F[_]: Async](driver: AsyncDriver[F]) extends UserRepository[F] {

  override def hasRootUser: F[Boolean] = {
    val query = "MATCH (u:User { root: true }) RETURN u LIMIT 1"
    query.query(ResultMapper.option(using ResultMapper.ignore)).single(driver).map(_.isDefined)
  }

  override def findUserByPreferredHandle(preferredHandle: String): F[Boolean] =
    c"MATCH (n:User { handle: $preferredHandle }) RETURN true AS exists LIMIT 1"
      .query(ResultMapper.option(using ResultMapper.boolean))
      .single(driver)
      .map(_.getOrElse(false))

  private def assertPreferredHandleUniqueness(user: User): F[Unit] =
    findUserByPreferredHandle(user.preferredHandle).flatMap { exists =>
      if exists then {
        Async[F].raiseError(
          new IllegalArgumentException(s"Already registered: ${user.preferredHandle}")
        )
      } else {
        Async[F].unit
      }
    }

  override def insertUser(user: LocalRegisteredUser): F[Unit] =
    // FIXME: TOCTOU
    for {
      _ <- assertPreferredHandleUniqueness(user)
      handle = user.preferredHandle
      id     = user.getIdentifier.raw.toString
      _ <- c"CREATE (n:User { handle: $handle, id: $id })".execute.void(driver)
    } yield ()

  override def createRootUser(user: LocalRegisteredUser): F[Unit] =
    // FIXME: TOCTOU
    for {
      _        <- assertPreferredHandleUniqueness(user)
      haveRoot <- this.hasRootUser
      _        <- Async[F].assert(haveRoot, "root user has been created!")
      handle = user.preferredHandle
      id     = user.getIdentifier.raw.toString
      _ <- c"CREATE (n:User:Person { handle: $handle, id: $id, root: true })".execute.void(driver)
    } yield ()
}
