using System.Text;
using Konscious.Security.Cryptography;

namespace KisaragiTech.Dape;

public static class PasswordGenerator
{
    public static byte[] CreateHashedPassword(string input)
    {
        var salt = SecureRNG.GenerateBytes(32);
        var password = Encoding.UTF8.GetBytes(input);

        var argon2 = new Argon2id(password)
        {
            Salt = salt,
            DegreeOfParallelism = 2,
            MemorySize = 65536,
            Iterations = 2,
        };

        return argon2.GetBytes(128);
    }
}