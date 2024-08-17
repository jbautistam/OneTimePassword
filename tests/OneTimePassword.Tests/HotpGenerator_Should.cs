using FluentAssertions;
using Bau.Libraries.OneTimePassword;

namespace OtpNet.Test;

/// <summary>
///     Pruebas de <see cref="HotpGenerator"/>
/// </summary>
public class HotpGenerator_Should
{
    // Secretos
    private const string Rfc6238SecretSha1 = "12345678901234567890";

    /// <summary>
    ///     Comprueba la generación de Hotp
    /// </summary>
    [Theory]
    [InlineData(BaseTokenGenerator.HashAlgorithm.Sha1, 0, "755224")]
    [InlineData(BaseTokenGenerator.HashAlgorithm.Sha1, 1, "287082")]
    [InlineData(BaseTokenGenerator.HashAlgorithm.Sha1, 2, "359152")]
    [InlineData(BaseTokenGenerator.HashAlgorithm.Sha1, 3, "969429")]
    [InlineData(BaseTokenGenerator.HashAlgorithm.Sha1, 4, "338314")]
    [InlineData(BaseTokenGenerator.HashAlgorithm.Sha1, 5, "254676")]
    [InlineData(BaseTokenGenerator.HashAlgorithm.Sha1, 6, "287922")]
    [InlineData(BaseTokenGenerator.HashAlgorithm.Sha1, 7, "162583")]
    [InlineData(BaseTokenGenerator.HashAlgorithm.Sha1, 8, "399871")]
    [InlineData(BaseTokenGenerator.HashAlgorithm.Sha1, 9, "520489")]
    public void compute_Hotp(BaseTokenGenerator.HashAlgorithm hash, long counter, string expected)
    {
        HotpGenerator hotp = new(Rfc6238SecretSha1, hash, expected.Length);

            // Comprueba el resultado
            hotp.Compute(counter).Should().Be(expected);
    }
}
