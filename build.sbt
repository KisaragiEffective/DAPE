val scala3Version = "3.7.4"

lazy val root = project
  .in(file("."))
  .settings(
    name := "dape",
    version := "0.1.0-SNAPSHOT",
    scalaVersion := scala3Version,
    scalacOptions ++= Seq(
      "-deprecation",
      "-feature",
      "-unchecked",
      "-Werror",
      "-Wunused:all",
      "-no-indent"
    ),
    libraryDependencies ++= Seq(
      // Core
      "org.typelevel" %% "cats-core" % "2.13.0",
      "org.typelevel" %% "cats-effect" % "3.6.3",

      // HTTP Server (ASP.NET Core equivalent)
      "org.http4s" %% "http4s-ember-server" % "0.23.30",
      "org.http4s" %% "http4s-dsl" % "0.23.30",
      "org.http4s" %% "http4s-circe" % "0.23.30",

      // JSON (System.Text.Json equivalent)
      "io.circe" %% "circe-core" % "0.14.10",
      "io.circe" %% "circe-generic" % "0.14.10",
      "io.circe" %% "circe-parser" % "0.14.10",

      // Neo4j (Neo4j.Driver equivalent)
      "io.github.neotypes" %% "neotypes-core" % "1.2.1",
      "io.github.neotypes" %% "neotypes-cats-effect" % "1.2.1",
      "org.neo4j.driver" % "neo4j-java-driver" % "6.0.3",

      // CLI Parser (CommandLineParser equivalent)
      "com.monovore" %% "decline" % "2.5.0",
      "com.monovore" %% "decline-effect" % "2.5.0",

      // Password Hashing (Konscious.Security.Cryptography.Argon2 equivalent)
      "com.password4j" % "password4j" % "1.8.2",

      // Testing
      "org.scalameta" %% "munit" % "1.0.4" % Test,
      "org.typelevel" %% "munit-cats-effect" % "2.1.0" % Test
    ),
    assembly / assemblyJarName := "dape.jar",
    assembly / mainClass := Some("io.github.ksrgtech.dape.Main"),
    assembly / assemblyMergeStrategy := {
      case PathList("META-INF", "services", _*) => MergeStrategy.concat
      case PathList("META-INF", _*) => MergeStrategy.discard
      case "reference.conf" => MergeStrategy.concat
      case _ => MergeStrategy.first
    }
  )
