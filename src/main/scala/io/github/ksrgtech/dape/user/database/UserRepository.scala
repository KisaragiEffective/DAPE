package io.github.ksrgtech.dape.user.database

import io.github.ksrgtech.dape.user.model.LocalRegisteredUser

trait UserRepository[F[_]] {
  def hasRootUser: F[Boolean]
  def findUserByPreferredHandle(preferredHandle: String): F[Boolean]
  def insertUser(user: LocalRegisteredUser): F[Unit]
  def createRootUser(user: LocalRegisteredUser): F[Unit]
}
