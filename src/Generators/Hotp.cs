namespace Bau.Libraries.OneTimePassword.Generators;

/// <summary>
///		Generador de token basado en HMAC (HOTP) (https://datatracker.ietf.org/doc/html/rfc4226)
/// </summary>
public class Hotp : BaseOtp
{
    public Hotp(string secretKey, OneTimePasswordGenerator.HashAlgorithm hashAlgorithm, int digits = 6) : base(secretKey, hashAlgorithm, Math.Clamp(digits, 6, 8)) {}

    /// <summary>
    ///     Calcula el token HOTP a partir de un contador
    /// </summary>
    public string ComputeHOTP(long counter) => Compute(counter);

    /// <summary>
    ///     Verifica un token (utiliza un algoritmo que siempre tarda el mismo tiempo)
    /// </summary>
    public bool VerifyHotp(string hotp, long counter) => ValuesEqual(hotp, ComputeHOTP(counter));
}
