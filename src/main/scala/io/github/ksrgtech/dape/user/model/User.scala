package io.github.ksrgtech.dape.user.model

import io.github.ksrgtech.dape.base.identity.Identifiable

trait User extends Identifiable[UserId] {
  def preferredHandle: String
}
