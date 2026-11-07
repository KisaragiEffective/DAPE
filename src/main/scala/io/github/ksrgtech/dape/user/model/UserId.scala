package io.github.ksrgtech.dape.user.model

import java.util.UUID

opaque type UserId = UUID

object UserId {
  def apply(raw: UUID): UserId = raw

  def generate(): UserId = UUID.randomUUID()

  extension (id: UserId) {
    def raw: UUID = id
  }
}
