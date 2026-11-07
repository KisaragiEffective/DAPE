package io.github.ksrgtech.dape.user.service

import cats.effect.kernel.Sync
import io.github.ksrgtech.dape.user.model.UserId

import java.util.UUID

object UserIdGenerationService {

  def generate[F[_]: Sync]: F[UserId] =
    Sync[F].delay {
      UserId(UUID.randomUUID())
    }
}
