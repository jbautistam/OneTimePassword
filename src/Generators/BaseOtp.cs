using System.Security.Cryptography;
using Bau.Libraries.OneTimePassword.TimeTools;

namespace Bau.Libraries.OneTimePassword.Generators;

/// <summary>
///     Cálculos básicos comunes para distintos generadores OTP
/// </summary>
public abstract class BaseOtp
{
    public BaseOtp(string secret, OneTimePasswordGenerator.HashAlgorithm hashAlgorithm, int digits)
    {
        Secret = secret;
        HashAlgorithm = hashAlgorithm;
        Digits = digits;
    }

    /// <summary>
    ///     Calcula el token a partir de un valor
    /// </summary>
    protected string Compute(long value) => TruncateDigits(CalculateOtp(value.GetBigEndian(), HashAlgorithm), Digits);

    /// <summary>
    ///     Calcula el valor del token
    /// </summary>
    private long CalculateOtp(byte[] data, OneTimePasswordGenerator.HashAlgorithm mode)
    {
        byte[] hmacComputed = ComputeHmac(Secret, HashAlgorithm, data);
        int offset = hmacComputed[hmacComputed.Length - 1] & 0x0F;

            // El RFC tiene un valor 19 fijo en la variable offset. Este método es similar, pero también sirve para SHA256 y SHA512
            // hmacComputedHash[19] => hmacComputedHash[hmacComputedHash.Length - 1]
            return (hmacComputed[offset] & 0x7f) << 24
                        | (hmacComputed[offset + 1] & 0xff) << 16
                        | (hmacComputed[offset + 2] & 0xff) << 8
                        | (hmacComputed[offset + 3] & 0xff);
    }

    /// <summary>
    ///     Trunca un número en X dígitos
    /// </summary>
    protected string TruncateDigits(long value, int digits)
    {
        int truncatedValue = (int) value % (int) Math.Pow(10, digits);

            // Convierte en cadena y añade 0s a la izquierda
            return truncatedValue.ToString().PadLeft(digits, '0');
    }

    /// <summary>
    ///     Comparación de cadenas utilizando siempre el mismo tiempo para evitar ataques
    /// </summary>
    protected bool ValuesEqual(string first, string second)
    {
        int result = 0;

            // Compara todos los caracteres de ambas cadenas
            for (int index = 0; index < first.Length; index++)
                if (index < second.Length)
                    result |= first[index] ^ second[index];
                else
                    result |= 1;
            // Ambas cadenas son iguales si el resultado es 0
            return result == 0;
    }

    /// <summary>
    ///     Utiliza el secreto para obtener un valor de hash utilizando el algoritmo espedificado
    /// </summary>
    private byte[] ComputeHmac(string secret, OneTimePasswordGenerator.HashAlgorithm hashAlgorithm, byte[] data)
    {
        using (HMAC hmac = CreateHmacHash(HashAlgorithm))
        {
            // Asigna la clave
            hmac.Key = System.Text.Encoding.UTF8.GetBytes(secret);
            // Calcula el valor hash
            return hmac.ComputeHash(data);
        }
    }

    /// <summary>
    ///     Crea un objeto para calcular el valor Hash (HMAC) dependiendo del algoritmo especificado
    /// </summary>
    private HMAC CreateHmacHash(OneTimePasswordGenerator.HashAlgorithm hashAlgorithm)
    {
        return hashAlgorithm switch
                    {
                        OneTimePasswordGenerator.HashAlgorithm.Sha256 => new HMACSHA256(),
                        OneTimePasswordGenerator.HashAlgorithm.Sha512 => new HMACSHA512(),
                        _ => new HMACSHA1(),
                    };
    }

    /// <summary>
    ///     Secreto
    /// </summary>
    public string Secret { get; }

    /// <summary>
    ///     Algoritmo de Hash a utilizar
    /// </summary>
    public OneTimePasswordGenerator.HashAlgorithm HashAlgorithm { get; }

    /// <summary>
    ///     Número de dígitos que debe tener el token devuelto
    /// </summary>
    public int Digits { get; }
}