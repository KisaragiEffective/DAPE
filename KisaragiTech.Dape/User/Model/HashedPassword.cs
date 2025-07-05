using System;
using System.Security.Cryptography;

namespace KisaragiTech.Dape.User.Model;

public sealed record HashedPassword(byte[] Raw)
{
    public bool Equals(HashedPassword? other)
    {
        return other != null && CryptographicOperations.FixedTimeEquals(this.Raw, other.Raw);
    }

    public override int GetHashCode()
    {
        return System.HashCode.Combine(Raw);
    }

    public string ToBase64()
    {
        return Convert.ToBase64String(Raw);
    }

    public static HashedPassword FromBase64(string base64)
    {
        ArgumentNullException.ThrowIfNull(base64);
        return new HashedPassword(Convert.FromBase64String(base64));
    }
}