package io.github.ksrgtech.dape.base.identity

object IdentifierComparator {

  def compare[T <: Identifiable[TIdentifier], TIdentifier](lhs: T, rhs: T): Boolean =
    lhs.getIdentifier == rhs.getIdentifier
}
