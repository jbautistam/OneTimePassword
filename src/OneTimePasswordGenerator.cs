namespace Bau.Libraries.OneTimePassword;

/// <summary>
///     Generador de tokens
/// </summary>
public class OneTimePasswordGenerator
{
    /// <summary>
    ///     Algoritmo de Hash a utilizar para obtener el HMAC
    /// </summary>
    public enum HashAlgorithm
    {
        /// <summary>Algoritmo Sha1</summary>
        Sha1,
        /// <summary>Algoritmo Sha256</summary>
        Sha256,
        /// <summary>Algoritmo Sha512</summary>
        Sha512
    }

    /// <summary>
    ///     Tipo de generador de tokens
    /// </summary>
    public enum GeneratorType
    {
        /// <summary>OTP basado en tiempo (https://datatracker.ietf.org/doc/html/rfc6238)</summary>
        Totp,
        /// <summary>OTP basado en HMAC (https://datatracker.ietf.org/doc/html/rfc4226)</summary>
        Hotp
    }

    public OneTimePasswordGenerator(string secret, HashAlgorithm algorithm = HashAlgorithm.Sha1)
    {
        Secret = secret;
        Algorithm = algorithm;
    }

    /// <summary>
    ///     Calcula un token utilizando TOTP
    /// </summary>
	public string ComputeTotp(long timestamp, int interval = 30, int length = 5)
	{
		Generators.Totp totp = new(Secret, Algorithm, length);

            totp.Interval = interval;
            return totp.Compute(TimeTools.UnixTime.GetDateTime(timestamp));
	}

    /// <summary>
    ///     Calcula un token utilizando TOTP
    /// </summary>
	public string ComputeTotp(DateTime? time = null, int interval = 30, int length = 5)
	{
		Generators.Totp totp = new(Secret, Algorithm, length);

            totp.Interval = interval;
            return totp.Compute(time);
	}

    /// <summary>
    ///     Calcula un token utilizando hOTP
    /// </summary>
	public string ComputeHotp(long counter, int length = 6)
	{
		Generators.Hotp hotp = new(Secret, Algorithm, length);

            return hotp.Compute(counter);
	}

    /// <summary>
    ///     Secreto
    /// </summary>
    public string Secret { get; }
    
    /// <summary>
    ///     Algoritmo
    /// </summary>
    public HashAlgorithm Algorithm { get; }
}
