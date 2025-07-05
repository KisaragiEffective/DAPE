using System.Text;
using KisaragiTech.Dape.Base.Random;
using KisaragiTech.Dape.User.Model;
using Konscious.Security.Cryptography;

namespace KisaragiTech.Dape;

public static class PasswordGenerator
{
    public static HashedPassword CreateHashedPassword(string input)
    {
        var salt = SecureRNG.GenerateBytes(32);
        var password = Encoding.UTF8.GetBytes(input);

        var argon2 = new Argon2id(password)
        {
            // TODO: 検証のときにもこれらのパスワードが必要！！
            Salt = salt,
            DegreeOfParallelism = 2,
            MemorySize = 65536,
            Iterations = 2,
        };

        return new HashedPassword(argon2.GetBytes(128));
    }
}