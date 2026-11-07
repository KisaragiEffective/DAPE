package io.github.ksrgtech.dape.config

import io.circe.Decoder
import io.circe.generic.semiauto.deriveDecoder

final case class DatabaseConfig(
    host: String,
    port: Int,
    user: String,
    password: String
)

object DatabaseConfig {
  implicit val decoder: Decoder[DatabaseConfig] = deriveDecoder[DatabaseConfig]
}
