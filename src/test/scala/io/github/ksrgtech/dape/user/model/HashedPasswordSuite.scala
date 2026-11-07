package io.github.ksrgtech.dape.user.model

import cats.effect.IO
import munit.CatsEffectSuite

class HashedPasswordSuite extends CatsEffectSuite {

  test("Colon0 - should fail with 0 colons") {
    val result = HashedPassword.deserialize("123")
    assert(result.isLeft)
    assert(result.left.exists(_.getMessage.contains("5 parts")))
  }

  test("Colon1 - should fail with 1 colon") {
    val result = HashedPassword.deserialize("123:123")
    assert(result.isLeft)
    assert(result.left.exists(_.getMessage.contains("5 parts")))
  }

  test("Colon2 - should fail with 2 colons") {
    val result = HashedPassword.deserialize("123:123:123")
    assert(result.isLeft)
    assert(result.left.exists(_.getMessage.contains("5 parts")))
  }

  test("Colon3 - should fail with 3 colons") {
    val result = HashedPassword.deserialize("123:123:123:123")
    assert(result.isLeft)
    assert(result.left.exists(_.getMessage.contains("5 parts")))
  }

  test("Colon4A - should fail with invalid base64 in first position") {
    val result = HashedPassword.deserialize("123:123:123:123:123")
    assert(result.isLeft)
    assert(result.left.exists(_.getMessage.contains("Invalid")))
  }

  test("Colon4B - should fail with invalid base64 in second position") {
    val result = HashedPassword.deserialize("SGVsbG8sIHdvcmxkIQ==:123:123:123:123")
    assert(result.isLeft)
    assert(result.left.exists(_.getMessage.contains("Invalid")))
  }

  test("Colon4C - should fail with invalid base64 in first position (valid in second)") {
    val result = HashedPassword.deserialize("123:SGVsbG8sIHdvcmxkIQ==:123:123:123")
    assert(result.isLeft)
    assert(result.left.exists(_.getMessage.contains("Invalid")))
  }

  test("Colon4D - should verify incorrectly with wrong password") {
    val result = HashedPassword.deserialize(
      "SGVsbG8sIHdvcmxkIUhlbGxvLCB3b3JsZCE=:SGVsbG8sIHdvcmxkIQ==:1:64:4"
    )
    assert(result.isRight)
    val hp = result.toOption.get
    hp.verify[IO]("Hello, world!").map { verified =>
      assert(!verified)
    }
  }

  test("Correct - should verify correctly with correct password") {
    val result = HashedPassword.deserialize(
      "/aRF7kSQ6MPJ8E7Tvim6r9zCMPZ53EJJxe0NfWLL6w2wogtrr/Vob1oc8gHMr54TWEauVgqL+4SNxCQzF52EKhS3LTSv21y3ih3hoP93ZAPzffuYNyLro/LChZNshNZbKHzHWfcER2FdwKT578AQu7kZadI7AFeCnjttiD1E5+I=:8nSdlDB3aDJNWs2Clp6QeP782ND10tMO6xV/scposvI=:2:65536:2"
    )
    assert(result.isRight)
    val hp = result.toOption.get
    hp.verify[IO]("Hello, world!").map { verified =>
      assert(verified)
    }
  }
}
