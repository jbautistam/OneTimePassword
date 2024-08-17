namespace Bau.Libraries.OneTimePassword.TimeTools;

/// <summary>
///     Clase de ayuda para manejar los intervalos de tiempo de generación de claves <see cref="TotpGenerator"/>
/// </summary>
public class TotpTimeManager
{
    // Constantes privadas
    private const long UnicEpocTicks = 621355968000000000L; // Número de ticks desde la medianoche del 1-1-1970 (Unix epoc)
    private const long TicksToSeconds = 10000000L; // Diviros para convertir ticks a segundos

    /// <summary>
    ///     Obtiene el paso en el que se encuentra partir de una fecha y un intervalo (por ejemplo, obtiene valores 
    /// </summary>
    internal long GetTimeStep(DateTime? timeStamp = null) => UnixTime.GetUnixTime(GetCorrectedTime(timeStamp)) / IntervalSeconds;

    /// <summary>
    ///     Aplica el factor de corrección a una fecha
    /// </summary>
    internal DateTime GetCorrectedTime(DateTime? timestamp = null) => (timestamp ?? DateTime.UtcNow) - TimeCorrectionFactor;

    /// <summary>
    ///     Segundos restantes en la ventana de tiempo actual
    /// </summary>
    public int RemainingSeconds(DateTime? timestamp = null)
    {
        return IntervalSeconds - ((int) ((GetCorrectedTime(timestamp).Ticks - UnicEpocTicks) / TicksToSeconds)) % IntervalSeconds;
    }

    /// <summary>
    ///     Intervalo (en segundos)
    /// </summary>
    public int IntervalSeconds { get; set; } = 30;

    /// <summary>
    ///     Factor de corrección de la fecha
    /// </summary>
    public TimeSpan TimeCorrectionFactor { get; set; } = TimeSpan.FromSeconds(0);
}