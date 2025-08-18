using System;

namespace KisaragiTech.Dape.Base.Random;

public static class SecureRNG
{
    public static byte[] GenerateBytes(int length)
    {
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        var ret = new byte[length];
        rng.GetBytes(ret);
        return ret;
    }

    public static void FillBytes(Span<byte> destination)
    {
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(destination);
    }
}