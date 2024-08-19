using System.Security.Cryptography;
using Bau.Libraries.OneTimePassword.TimeTools;

namespace Bau.Libraries.OneTimePassword;

/// <summary>
///     Clase base para los diferentes generadores de OTP
/// </summary>
public abstract class BaseTokenGenerator
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

    protected BaseTokenGenerator(Secret secret, HashAlgorithm algorithm, int digits)
    {
        Secret = secret;
        Algorithm = algorithm;
        Digits = digits;
    }

    /// <summary>
    ///     Calcula el token a partir de un valor
    /// </summary>
    protected string ComputeToken(long value) => TruncateDigits(Compute(value.ToBigEndian(), Algorithm), Digits);

    /// <summary>
    ///     Calcula el valor del token
    /// </summary>
    private long Compute(byte[] data, HashAlgorithm algorithm)
    {
        byte[] hmacComputed = ComputeHmac(Secret, algorithm, data);
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
    private string TruncateDigits(long value, int digits)
    {
        int truncated = (int) value % (int) Math.Pow(10, digits);

            // Convierte el número en cadena y añade 0s a la izquierda
            return truncated.ToString().PadLeft(digits, '0');
    }

    /// <summary>
    ///     Comparación de cadenas utilizando siempre el mismo tiempo para evitar ataques
    /// </summary>
    protected bool CompareTokens(string first, string second)
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
    private byte[] ComputeHmac(Secret secret, HashAlgorithm algorithm, byte[] data)
    {
        using (HMAC hmac = GetHmac(algorithm))
        {
            // Asigna la clave
            hmac.Key = secret.Decode();
            // Calcula el valor hash
            return hmac.ComputeHash(data);
        }
    }

    /// <summary>
    ///     Crea un objeto para calcular el valor Hash (HMAC) dependiendo del algoritmo especificado
    /// </summary>
    private HMAC GetHmac(HashAlgorithm algorithm)
    {
        return algorithm switch
                    {
                        HashAlgorithm.Sha256 => new HMACSHA256(),
                        HashAlgorithm.Sha512 => new HMACSHA512(),
                        _ => new HMACSHA1()
                    };
    }

    /// <summary>
    ///     Secreto
    /// </summary>
    public Secret Secret { get; }

    /// <summary>
    ///     Algoritmo de Hash a utilizar
    /// </summary>
    public HashAlgorithm Algorithm { get; }

    /// <summary>
    ///     Número de dígitos que debe tener el token devuelto
    /// </summary>
    public int Digits { get; }
}