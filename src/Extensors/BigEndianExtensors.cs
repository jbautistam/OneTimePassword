namespace Bau.Libraries.OneTimePassword.TimeTools;

/// <summary>
///     Métodos para obtener valores numéricos como big endian.
///     .Net utiliza números little endian, por tanto debemos revertir el orden de los bytes
/// </summary>
internal static class BigEndianExtensors
{
    /// <summary>
    ///     Convierte un long en un array big endian (RFC 4226) 
    /// </summary>
    internal static byte[] GetBigEndian(this long input) => Reverse(BitConverter.GetBytes(input));

    /// <summary>
    ///     Convierte un int en un array big endian (RFC 4226)
    /// </summary>
    internal static byte[] GetBigEndian(this int input) => Reverse(BitConverter.GetBytes(input));

    /// <summary>
    ///     Da la vuelta a un array de bytes
    /// </summary>
    private static byte[] Reverse(byte[] buffer)
    {
        // Da la vuelta al buffer
        Array.Reverse(buffer);
        // y devuelve el valor
        return buffer;
    }
}
