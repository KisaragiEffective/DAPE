package io.github.ksrgtech.dape.user.model

import cats.effect.kernel.Sync

/** A fully registered local user who can perform various operations if they have the necessary permissions.
  */
final case class LocalRegisteredUser(
    identifier: UserId,
    preferredHandle: String,
    hashedPassword: HashedPassword
) extends LocalUser {
  override def getIdentifier: UserId = identifier

  def verifyPassword[F[_]: Sync](raw: String): F[Boolean] =
    hashedPassword.verify[F](raw)
}
