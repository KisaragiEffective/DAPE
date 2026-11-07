package io.github.ksrgtech.dape.user.model

/** A tentatively registered local user. The only things a tentative user can do are: complete the registration, or be
  * removed from the queue after a timeout.
  *
  * Use [[io.github.ksrgtech.dape.user.service.TentativeUserFactory.create]] to create instances.
  */
final class LocalTentativeUser private[user] (
    private val identifier: UserId,
    val preferredHandle: String
) extends LocalUser {
  override def getIdentifier: UserId = identifier

  def toRegisteredUser(hashedPassword: HashedPassword): LocalRegisteredUser =
    LocalRegisteredUser(identifier, preferredHandle, hashedPassword)
}
