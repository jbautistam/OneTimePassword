using FluentAssertions;
using Bau.Libraries.OneTimePassword;

namespace OneTimePassword.Tests;

/// <summary>
///     Pruebas de <see cref="HotpGenerator"/>
/// </summary>
public class HotpGenerator_Should
{
    /// <summary>
    ///     Comprueba la generación de Hotp (los datos de la teoría se generan con <see cref="Tools.TestGenerator"/>)
    /// </summary>
    [Theory]
    [InlineData("suEINrBd", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, 170642, "730584")]
    [InlineData("vlkJptZE", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha256, 723019, "7929845")]
    [InlineData("HHYTfCpg", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha512, 106205, "9798121")]
    [InlineData("nsbhLfRd", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha1, 463125, "2171392")]
    [InlineData("qZGESSmV", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha256, 663081, "1522676")]
    [InlineData("FFQMLNIK", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha512, 638098, "6166610")]
    [InlineData("NjqlmBEk", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha1, 183209, "8656231")]
    [InlineData("BiPJdCKN", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha256, 135890, "3337489")]
    [InlineData("xdeKhHhE", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha512, 877220, "7022811")]
    [InlineData("JMhXssHK", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, 813399, "5289934")]
    [InlineData("VIvgtRpn", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha256, 169718, "542784")]
    [InlineData("BzGEpRUE", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha512, 627909, "122204")]
    [InlineData("kFgSYPQC", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha1, 49045, "5275067")]
    [InlineData("UunXYyAA", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha256, 527359, "386343")]
    [InlineData("NUmkKVbU", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha512, 304018, "817798")]
    [InlineData("WPesfftl", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha1, 598916, "8654595")]
    [InlineData("dDHGyrtW", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha256, 987906, "464328")]
    [InlineData("ILFyvWJA", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha512, 168814, "0193438")]
    [InlineData("RemvEdKs", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, 510584, "236033")]
    [InlineData("NtMEPqDC", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha256, 842258, "3193452")]
    [InlineData("ZlIBfadG", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha512, 444588, "6370955")]
    [InlineData("AYnSGaDe", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha1, 860188, "6873156")]
    [InlineData("TFaYngCW", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha256, 349402, "663296")]
    [InlineData("nmdydvPz", Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha512, 837180, "6806500")]
    [InlineData("VAuUhppk", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha1, 752560, "0269126")]
    [InlineData("jdhjBcnA", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha256, 517396, "3275452")]
    [InlineData("LXmpHHlw", Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha512, 572793, "1628292")]
    public void compute_Hotp(string key, Secret.Encoding encoding, BaseTokenGenerator.HashAlgorithm algorithm, long counter, string expected)
    {
        HotpGenerator hotp = new(key, encoding, algorithm, expected.Length);

            // Comprueba el resultado
            hotp.Compute(counter).Should().Be(expected);
    }
}
