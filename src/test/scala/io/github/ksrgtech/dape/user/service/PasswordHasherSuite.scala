package io.github.ksrgtech.dape.user.service

import cats.effect.IO
import munit.CatsEffectSuite

class PasswordHasherSuite extends CatsEffectSuite {

  test("IdentityCase - should hash and verify password correctly") {
    for {
      password  <- PasswordHasher.createHashedPassword[IO]("Hello, world!")
      isCorrect <- password.verify[IO]("Hello, world!")
    } yield assert(isCorrect)
  }
}
