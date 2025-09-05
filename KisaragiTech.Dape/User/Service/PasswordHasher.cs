using System.Text;
using KisaragiTech.Dape.Base.Random;
using KisaragiTech.Dape.User.Model;
using Konscious.Security.Cryptography;

namespace KisaragiTech.Dape.User.Service;

public static class PasswordHasher
{
    public static HashedPassword CreateHashedPassword(string input)
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

        return new HashedPassword(argon2.GetBytes(128), salt, 2, 65536, 2);
    }
}
