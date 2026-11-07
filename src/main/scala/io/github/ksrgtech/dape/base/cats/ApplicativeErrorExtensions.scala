package io.github.ksrgtech.dape.base.cats

import cats.ApplicativeError

extension [F[_]](ae: ApplicativeError[F, Throwable]) {

  inline def assert(condition: Boolean, message: => String) /* (using CanThrow[IllegalArgumentException]) */: F[Unit] =
    if condition then ae.unit
    else ae.raiseError(new IllegalArgumentException(message))
}
