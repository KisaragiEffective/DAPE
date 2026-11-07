package io.github.ksrgtech.dape.config

import cats.MonadThrow
import io.circe.Decoder
import io.circe.generic.semiauto.deriveDecoder
import io.circe.parser.decode

final case class RootConfig(
    database: DatabaseConfig
)

object RootConfig {
  implicit val decoder: Decoder[RootConfig] = deriveDecoder[RootConfig]

  def deserializeFromJson[F[_]: MonadThrow](json: String): F[RootConfig] =
    MonadThrow[F].fromEither(
      decode[RootConfig](json).left.map { err =>
        new IllegalArgumentException(s"Failed to parse config: ${err.getMessage}")
      }
    )
}
