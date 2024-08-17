namespace Bau.Libraries.OneTimePassword.TimeTools;

/// <summary>
///     Clase para medir el número de segundos pasados desde la medianoche del 1-1-1970 (UnixTime o POSIX Time)
/// </summary>
internal static class UnixTime
{
    // Constantes privadas
    private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    ///     Convierte la fecha actual a tiempo Unix
    /// </summary>
    internal static long GetUnixTime() => GetUnixTime(DateTime.UtcNow);

    /// <summary>
    ///     Convierte una fecha a tiempo Unix
    /// </summary>
    internal static long GetUnixTime(DateTime dateTime) => (long) (dateTime - Epoch).TotalSeconds;

    /// <summary>
    ///     Convierte un tiempo Unix a una fecha UTC
    /// </summary>
    internal static DateTime GetDateTime(long unixTime) => Epoch.AddSeconds(unixTime).ToUniversalTime();
}