package io.github.ksrgtech.dape.base.identity

trait Identifiable[+TIdentifier] {
  def getIdentifier: TIdentifier
}
