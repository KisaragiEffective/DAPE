using System;

namespace KisaragiTech.Dape.Base.Identity;

public static class IdentifierComparator
{
    public static bool Compare<T, TIdentifier>(T lhs, T rhs)
        where T : IIdentifiable<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier> =>
        lhs.GetIdentifier().Equals(rhs.GetIdentifier());
}
