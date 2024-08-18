using FluentAssertions;
using Bau.Libraries.OneTimePassword;

namespace OneTimePassword.Tests;

/// <summary>
///     Pruebas de <see cref="TotpGenerator"/>
/// </summary>
public class TotpGenerator_Should
{
    /// <summary>
    ///     Comprueba la generación de diferentes tokens TOTP (los datos de la teoría se generan con <see cref="Tools.TestGenerator"/>)
    /// </summary>
    [Theory]
    [InlineData("jzTWnwWR", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, 765695, "773186")]
    [InlineData("kTuWcaLf", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha256, 785936, "592968")]
    [InlineData("yzTEhhtF", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha512, 418809, "087406")]
    [InlineData("gYyUDDse", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha1, 11198, "623305")]
    [InlineData("prmKausx", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha256, 95984, "087973")]
    [InlineData("GDQtnPvX", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha512, 785723, "7635947")]
    [InlineData("rDmqUUzL", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha1, 252306, "809211")]
    [InlineData("FpRUNVLa", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha256, 49181, "282580")]
    [InlineData("wanaRAuz", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha512, 63537, "9268158")]
    [InlineData("VqlclujF", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, 768194, "983932")]
    [InlineData("UUdLChSP", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha256, 573303, "198113")]
    [InlineData("yghjftLC", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha512, 928872, "669169")]
    [InlineData("vVIYHUUb", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha1, 21055, "879200")]
    [InlineData("KBbsbhAx", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha256, 379631, "1170673")]
    [InlineData("ecLZnfmT", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha512, 323902, "8668954")]
    [InlineData("mwxbFnPT", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha1, 131593, "463845")]
    [InlineData("PYurZnPE", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha256, 673888, "6241346")]
    [InlineData("DzxaDcwn", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha512, 627855, "234126")]
    [InlineData("FEfsmfnV", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, 99646, "659599")]
    [InlineData("gUqvTzGh", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha256, 37302, "2784589")]
    [InlineData("ByHNtfqF", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha512, 32324, "8272361")]
    [InlineData("bQBPfFPP", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha1, 711103, "314606")]
    [InlineData("IleuVBtp", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha256, 461049, "3834096")]
    [InlineData("SCFqQZNx", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha512, 541723, "1245032")]
    [InlineData("GPrbUIsV", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha1, 929609, "522769")]
    [InlineData("Mpbbvtdb", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha256, 638138, "0941388")]
    [InlineData("ltGAtgFJ", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha512, 574173, "160236")]
    public void compute_Totp(string key, Secret.Encoding encoding, BaseTokenGenerator.HashAlgorithm algorithm, long timestamp, string expected)
    {
        TotpGenerator totp = new(key, encoding, algorithm, expected.Length);

            // Asigna el intervalo
            totp.TimeManager.IntervalSeconds = 30;
            // Comprueba el resultado
            totp.Compute(timestamp).Should().Be(expected);
    }
}