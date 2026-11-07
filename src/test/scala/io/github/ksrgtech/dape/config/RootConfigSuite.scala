package io.github.ksrgtech.dape.config

import munit.FunSuite

class RootConfigSuite extends FunSuite {

  test("deserializeFromJson should work with Either MonadThrow instance") {
    val json =
      """{"database":{"host":"localhost","port":12345,"user":"dape","password":"secret"}}"""
    val parsed = RootConfig.deserializeFromJson[[x] =>> Either[Throwable, x]](json)
    assertEquals(
      parsed,
      Right(RootConfig(DatabaseConfig("localhost", 12345, "dape", "secret")))
    )
  }

  test("deserializeFromJson should fail with IllegalArgumentException for invalid json") {
    val parsed = RootConfig.deserializeFromJson[[x] =>> Either[Throwable, x]]("""{"database":{}}""")
    assert(parsed.isLeft, parsed)
    assert(parsed.left.exists(_.isInstanceOf[IllegalArgumentException]), parsed)
  }
}
