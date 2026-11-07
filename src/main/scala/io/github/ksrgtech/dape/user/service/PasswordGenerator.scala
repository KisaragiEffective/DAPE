package io.github.ksrgtech.dape.user.service

import cats.MonadThrow
import cats.effect.std.SecureRandom
import cats.implicits.given

import io.github.ksrgtech.dape.base.cats.assert

object PasswordGenerator {

  // Character set (excluding l, 1, O, 0 for readability)
  private val Alphabet: String =
    // Uppercase (excluding O)
    "ABCDEFGHJKLMNPQRSTUVWXYZ" +
      // Lowercase (excluding l)
      "abcdefghijkmnopqrstuvwxyz" +
      // Numbers (excluding 0 and 1)
      "23456789" +
      // Symbols (adjustable)
      "!@#$%^&*()-_=+[]{}|;:,.<>?"

  def generate[F[_]: {SecureRandom, MonadThrow as mt}](length: Int = 20): F[String] = for {
    _ <- mt.assert(length > 0, "length must be positive")
    sb = new StringBuilder(length)
    cc <- SecureRandom[F]
      .nextIntBounded(Alphabet.length)
      .map(Alphabet.charAt)
      .replicateA(length)
  } yield sb.appendAll(cc).toString()
}
