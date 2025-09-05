using System;
using System.Security.Cryptography;
using System.Text;

namespace KisaragiTech.Dape.User.Service;

public static class PasswordGenerator
{
    // 文字集合（視認性も考慮してl,1,O,0は除外）
    private const string Alphabet =
        // 大文字 (O除く)
        "ABCDEFGHJKLMNPQRSTUVWXYZ" +
        // 小文字 (l除く)
        "abcdefghijkmnopqrstuvwxyz" +
        // 数字 (0と1除く)
        "23456789" +
        // 記号（調整可）
        "!@#$%^&*()-_=+[]{}|;:,.<>?";

    public static string Generate(int length = 20)
    {
        if (length <= 0)
        {
            throw new ArgumentException("length must be positive", nameof(length));
        }

        var sb = new StringBuilder(length);

        for (var i = 0; i < length; i++)
        {
            var index = RandomNumberGenerator.GetInt32(Alphabet.Length);
            sb.Append(Alphabet[index]);
        }

        return sb.ToString();
    }
}
