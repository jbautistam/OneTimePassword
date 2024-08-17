namespace Bau.Libraries.OneTimePassword;

/// <summary>
///		Generador de token basado en HMAC (HOTP) (https://datatracker.ietf.org/doc/html/rfc4226)
/// </summary>
public class HotpGenerator : BaseTokenGenerator
{
    public HotpGenerator(string secretKey, HashAlgorithm hashAlgorithm, int digits = 6) : base(secretKey, hashAlgorithm, Math.Clamp(digits, 6, 8)) {}

    /// <summary>
    ///     Calcula el token HOTP a partir de un contador
    /// </summary>
    public string Compute(long counter) => ComputeToken(counter);

    /// <summary>
    ///     Verifica un token (utiliza un algoritmo que siempre tarda el mismo tiempo)
    /// </summary>
    public bool Verify(string token, long counter) => ValuesEqual(token, Compute(counter));
}
