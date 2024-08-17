using FluentAssertions;
using Bau.Libraries.OneTimePassword;

namespace OtpNet.Test;

/// <summary>
///     Pruebas de <see cref="TotpGenerator"/>
/// </summary>
public class TotpGenerator_Should
{
    // Secretos para los diferentes algorigmos
    private const string Rfc6238SecretSha1 = "12345678901234567890";
    private const string Rfc6238SecretSha256 = "12345678901234567890123456789012";
    private const string Rfc6238SecretSha512 = "1234567890123456789012345678901234567890123456789012345678901234";

    /// <summary>
    ///     Comprueba la generación de diferentes tokens TOTP
    /// </summary>
    [Theory]
    [InlineData(Rfc6238SecretSha1, BaseTokenGenerator.HashAlgorithm.Sha1, 59, "94287082")]
    [InlineData(Rfc6238SecretSha256, BaseTokenGenerator.HashAlgorithm.Sha256, 59, "46119246")]
    [InlineData(Rfc6238SecretSha512, BaseTokenGenerator.HashAlgorithm.Sha512, 59, "90693936")]
    [InlineData(Rfc6238SecretSha1, BaseTokenGenerator.HashAlgorithm.Sha1, 1111111109, "07081804")]
    [InlineData(Rfc6238SecretSha256, BaseTokenGenerator.HashAlgorithm.Sha256, 1111111109, "68084774")]
    [InlineData(Rfc6238SecretSha512, BaseTokenGenerator.HashAlgorithm.Sha512, 1111111109, "25091201")]
    [InlineData(Rfc6238SecretSha1, BaseTokenGenerator.HashAlgorithm.Sha1, 1111111111, "14050471")]
    [InlineData(Rfc6238SecretSha256, BaseTokenGenerator.HashAlgorithm.Sha256, 1111111111, "67062674")]
    [InlineData(Rfc6238SecretSha512, BaseTokenGenerator.HashAlgorithm.Sha512, 1111111111, "99943326")]
    [InlineData(Rfc6238SecretSha1, BaseTokenGenerator.HashAlgorithm.Sha1, 1234567890, "89005924")]
    [InlineData(Rfc6238SecretSha256, BaseTokenGenerator.HashAlgorithm.Sha256, 1234567890, "91819424")]
    [InlineData(Rfc6238SecretSha512, BaseTokenGenerator.HashAlgorithm.Sha512, 1234567890, "93441116")]
    [InlineData(Rfc6238SecretSha1, BaseTokenGenerator.HashAlgorithm.Sha1, 2000000000, "69279037")]
    [InlineData(Rfc6238SecretSha256, BaseTokenGenerator.HashAlgorithm.Sha256, 2000000000, "90698825")]
    [InlineData(Rfc6238SecretSha512, BaseTokenGenerator.HashAlgorithm.Sha512, 2000000000, "38618901")]
    [InlineData(Rfc6238SecretSha1, BaseTokenGenerator.HashAlgorithm.Sha1, 20000000000, "65353130")]
    [InlineData(Rfc6238SecretSha256, BaseTokenGenerator.HashAlgorithm.Sha256, 20000000000, "77737706")]
    [InlineData(Rfc6238SecretSha512, BaseTokenGenerator.HashAlgorithm.Sha512, 20000000000, "47863826")]
    [InlineData(Rfc6238SecretSha1, BaseTokenGenerator.HashAlgorithm.Sha1, 20000000000, "353130")]
    [InlineData(Rfc6238SecretSha256, BaseTokenGenerator.HashAlgorithm.Sha256, 20000000000, "737706")]
    [InlineData(Rfc6238SecretSha512, BaseTokenGenerator.HashAlgorithm.Sha512, 20000000000, "863826")]
    public void compute_Totp(string secret, BaseTokenGenerator.HashAlgorithm algorithm, long timestamp, string expected)
    {
        TotpGenerator totp = new(secret, algorithm, expected.Length);

            // Asigna el intervalo
            totp.TimeManager.IntervalSeconds = 30;
            // Comprueba el resultado
            totp.Compute(timestamp).Should().Be(expected);
    }
}
