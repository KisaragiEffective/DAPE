package io.github.ksrgtech.dape.user.model

import cats.effect.kernel.Sync
import com.password4j.Argon2Function
import io.github.ksrgtech.dape.base.strings.StringSplitter

import java.security.MessageDigest
import java.util.Base64
import scala.util.Try

final case class HashedPassword(
    hash: Array[Byte],
    salt: Array[Byte],
    iterations: Int,
    memorySize: Int,
    degreeOfParallelism: Int
) {

  def toSerializationFormat: String = {
    val encoder = Base64.getEncoder
    s"${encoder.encodeToString(this.hash)}:${encoder.encodeToString(this.salt)}:${this.iterations}:${this.memorySize}:${this.degreeOfParallelism}"
  }

  def verify[F[_]: Sync](password: String): F[Boolean] =
    Sync[F].delay {
      val argon2 = Argon2Function.getInstance(
        this.memorySize,
        this.iterations,
        this.degreeOfParallelism,
        HashedPassword.HashLength,
        com.password4j.types.Argon2.ID
      )
      val computed = argon2.hash(password.getBytes("UTF-8"), this.salt).getBytes
      MessageDigest.isEqual(computed, this.hash)
    }

  def verifyUnsafe(password: String): Boolean = {
    val argon2 = Argon2Function.getInstance(
      this.memorySize,
      this.iterations,
      this.degreeOfParallelism,
      HashedPassword.HashLength,
      com.password4j.types.Argon2.ID
    )
    val computed = argon2.hash(password.getBytes("UTF-8"), this.salt).getBytes
    MessageDigest.isEqual(computed, this.hash)
  }

  def equalsPassword(other: HashedPassword): Boolean =
    MessageDigest.isEqual(this.hash, other.hash)
}

object HashedPassword {
  private val HashLength: Int = 128
  private val Base64Pattern   = "^[A-Za-z0-9+/]*={0,2}$".r

  private def decodeBase64Strict(input: String): Either[IllegalArgumentException, Array[Byte]] =
    // Check that the string is valid base64 format
    // Base64 encoded strings should have length that is a multiple of 4
    if input.isEmpty then {
      Left(new IllegalArgumentException("Invalid base64: empty string"))
    } else if input.length % 4 != 0 then {
      Left(new IllegalArgumentException(s"Invalid base64: length ${input.length} is not a multiple of 4"))
    } else if !Base64Pattern.matches(input) then {
      Left(new IllegalArgumentException("Invalid base64: contains invalid characters"))
    } else {
      Try(Base64.getDecoder.decode(input)).toEither.left.map { e =>
        new IllegalArgumentException(s"Invalid base64: ${e.getMessage}")
      }
    }

  def deserialize(serialized: String): Either[IllegalArgumentException, HashedPassword] = {
    val parts = StringSplitter.toSubstrings(serialized, ':').toList
    parts match {
      case hashStr :: saltStr :: iterStr :: memStr :: parallelStr :: Nil =>
        for {
          hash <- decodeBase64Strict(hashStr).left.map { e =>
            new IllegalArgumentException(s"Invalid hash format: ${e.getMessage}")
          }
          salt <- decodeBase64Strict(saltStr).left.map { e =>
            new IllegalArgumentException(s"Invalid salt format: ${e.getMessage}")
          }
          iterations <- Try(iterStr.toInt).toEither.left.map { e =>
            new IllegalArgumentException(s"Invalid iterations format: ${e.getMessage}")
          }
          memorySize <- Try(memStr.toInt).toEither.left.map { e =>
            new IllegalArgumentException(s"Invalid memory size format: ${e.getMessage}")
          }
          parallelism <- Try(parallelStr.toInt).toEither.left.map { e =>
            new IllegalArgumentException(s"Invalid parallelism format: ${e.getMessage}")
          }
        } yield HashedPassword(hash, salt, iterations, memorySize, parallelism)
      case _ =>
        Left(new IllegalArgumentException("Hashed password must contain 5 parts exactly."))
    }
  }

  def unsafeDeserialize(serialized: String): HashedPassword =
    deserialize(serialized) match {
      case Right(hp) => hp
      case Left(e)   => throw e
    }
}
