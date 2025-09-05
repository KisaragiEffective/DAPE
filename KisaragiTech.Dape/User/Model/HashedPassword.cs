using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using KisaragiTech.Dape.Base.Strings;
using Konscious.Security.Cryptography;

namespace KisaragiTech.Dape.User.Model;

public sealed record HashedPassword(ReadOnlyMemory<byte> Hash, ReadOnlyMemory<byte> Salt, int Iterations, int MemorySize, int DegreeOfParallelism)
{
    public bool Equals(HashedPassword? other) => other != null && CryptographicOperations.FixedTimeEquals(this.Hash.Span, other.Hash.Span);

    public override int GetHashCode() => System.HashCode.Combine(this.Hash);

    public string ToSerializationFormat() => Convert.ToBase64String(this.Hash.Span) + ":" + Convert.ToBase64String(this.Salt.Span) + ":" + this.Iterations + ":" + this.MemorySize + ":" + this.DegreeOfParallelism;

    public static HashedPassword Deserialize(string serialized)
    {
        var parts = StringSplitter.ToMemories(serialized, ':').ToList();
        return parts switch
        {
            [var hash, var salt, var iterations, var memorySize, var degreeOfParallelism] => new HashedPassword(
                Convert.FromBase64String(hash.ToString()),
                Convert.FromBase64String(salt.ToString()),
                int.Parse(iterations.Span, NumberStyles.Integer, CultureInfo.InvariantCulture),
                int.Parse(memorySize.Span, NumberStyles.Integer, CultureInfo.InvariantCulture),
                int.Parse(degreeOfParallelism.Span, NumberStyles.Integer, CultureInfo.InvariantCulture)),
            _ => throw new InvalidOperationException("Hashed password must contain 5 parts exactly."),
        };
    }

    public bool Verify(string password)
    {
        var salt = this.Salt.ToArray();
        using var argon2 = new Argon2id(System.Text.Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            Iterations = this.Iterations,
            MemorySize = this.MemorySize,
            DegreeOfParallelism = this.DegreeOfParallelism,
        };
        var computed = argon2.GetBytes(this.Hash.Length);
        return CryptographicOperations.FixedTimeEquals(computed, this.Hash.Span);
    }
}
