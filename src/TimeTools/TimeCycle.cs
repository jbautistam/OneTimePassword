namespace Bau.Libraries.OneTimePassword.TimeTools;

/// <summary>
///     Clase de ayuda para obtener los ciclos de tiempo
/// </summary>
internal static class TimeCycle
{
    ///// <summary>
    /////     Obtiene el siguiente ciclo de tiempo (a partir de la fecha actual con intervalo de 30 segundos)
    ///// </summary>
    //internal static DateTime GetNextCycleTimeUtc() => GetNextCycleTimeUtc(DateTime.UtcNow, 30);

    ///// <summary>
    /////     Obtiene el siguiente ciclo de tiempo
    ///// </summary>
    //internal static DateTime GetNextCycleTimeUtc(DateTime dateTime, int seconds) => UnixTime.GetDateTime((UnixTime.GetUnixTime(dateTime) / seconds + 1) * seconds);

    ///// <summary>
    /////     Obtiene el paso en el que se encuentra partir de una fecha y un intervalo (por ejemplo, obtiene valores 
    ///// </summary>
    //internal static long GetTimeStep(DateTime dateTime, int interval) => UnixTime.GetUnixTime(dateTime) / interval;
}