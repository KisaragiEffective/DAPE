using System;
using KisaragiTech.Dape.User.Model;

namespace KisaragiTech.Dape.Test.User.Model;

public static class HashedPasswordTests
{
    [Fact]
    private static void Colon0()
    {
        Assert.Throws<InvalidOperationException>(() => HashedPassword.Deserialize("123"));
    }

    [Fact]
    private static void Colon1()
    {
        Assert.Throws<InvalidOperationException>(() => HashedPassword.Deserialize("123:123"));
    }

    [Fact]
    private static void Colon2()
    {
        Assert.Throws<InvalidOperationException>(() => HashedPassword.Deserialize("123:123:123"));
    }

    [Fact]
    private static void Colon3()
    {
        Assert.Throws<InvalidOperationException>(() => HashedPassword.Deserialize("123:123:123:123"));
    }

    [Fact]
    private static void Colon4A()
    {
        Assert.Throws<FormatException>(() => HashedPassword.Deserialize("123:123:123:123:123"));
    }

    [Fact]
    private static void Colon4B()
    {
        Assert.Throws<FormatException>(() => HashedPassword.Deserialize("SGVsbG8sIHdvcmxkIQ==:123:123:123:123"));
    }

    [Fact]
    private static void Colon4C()
    {
        Assert.Throws<FormatException>(() => HashedPassword.Deserialize("123:SGVsbG8sIHdvcmxkIQ==:123:123:123"));
    }

    [Fact]
    private static void Colon4D()
    {
        var a = HashedPassword.Deserialize("SGVsbG8sIHdvcmxkIUhlbGxvLCB3b3JsZCE=:SGVsbG8sIHdvcmxkIQ==:1:64:4");
        Assert.False(a.Verify("Hello, world!"));
    }

    [Fact]
    private static void Correct()
    {
        var a = HashedPassword.Deserialize(
            "/aRF7kSQ6MPJ8E7Tvim6r9zCMPZ53EJJxe0NfWLL6w2wogtrr/Vob1oc8gHMr54TWEauVgqL+4SNxCQzF52EKhS3LTSv21y3ih3hoP93ZAPzffuYNyLro/LChZNshNZbKHzHWfcER2FdwKT578AQu7kZadI7AFeCnjttiD1E5+I=:8nSdlDB3aDJNWs2Clp6QeP782ND10tMO6xV/scposvI=:2:65536:2");
        Assert.True(a.Verify("Hello, world!"));
    }
}
