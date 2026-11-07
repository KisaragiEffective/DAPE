package io.github.ksrgtech.dape.user.service

import cats.effect.kernel.Sync
import cats.implicits.*
import io.github.ksrgtech.dape.user.model.LocalTentativeUser

object TentativeUserFactory {

  def create[F[_]: Sync](initialPreferredUsername: String): F[LocalTentativeUser] =
    UserIdGenerationService.generate[F].map { userId =>
      new LocalTentativeUser(userId, initialPreferredUsername)
    }
}
