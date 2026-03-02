val scala3Version = "3.8.2"

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
      "org.typelevel" %% "cats-effect" % "3.7-4972921",

      // HTTP Server (ASP.NET Core equivalent)
      "org.http4s" %% "http4s-ember-server" % "0.23.33",
      "org.http4s" %% "http4s-dsl" % "0.23.33",
      "org.http4s" %% "http4s-circe" % "0.23.33",

      // JSON (System.Text.Json equivalent)
      "io.circe" %% "circe-core" % "0.14.15",
      "io.circe" %% "circe-generic" % "0.14.15",
      "io.circe" %% "circe-parser" % "0.14.15",

      // Neo4j (Neo4j.Driver equivalent)
      "io.github.neotypes" %% "neotypes-core" % "1.2.2",
      "io.github.neotypes" %% "neotypes-cats-effect" % "1.2.2",
      "org.neo4j.driver" % "neo4j-java-driver" % "5.28.10",

      // CLI Parser (CommandLineParser equivalent)
      "com.monovore" %% "decline" % "2.6.0",
      "com.monovore" %% "decline-effect" % "2.6.0",

      // Password Hashing (Konscious.Security.Cryptography.Argon2 equivalent)
      "com.password4j" % "password4j" % "1.8.4",

      // Testing
      "org.scalameta" %% "munit" % "1.2.3" % Test,
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
