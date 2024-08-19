namespace Bau.Libraries.OneTimePassword.TimeTools;

/// <summary>
///     Clase de ayuda para manejar los intervalos de tiempo de generación de claves <see cref="TotpGenerator"/>
/// </summary>
public class TotpTimeManager
{
    // Constantes privadas
    private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // Fecha de inicio de la hora Unix / Posix
    private static readonly long EpochTicks = Epoch.Ticks; // Número de ticks desde la medianoche del 1-1-1970 (Unix epoc)
    private const long TicksToSeconds = 10_000_000L; // Divisor para convertir ticks a segundos

    /// <summary>
    ///     Obtiene el paso en el que se encuentra partir de los ticks de Unix
    /// </summary>
    internal long GetTimeStep(long timestamp) => timestamp / IntervalSeconds;

    /// <summary>
    ///     Obtiene el paso en el que se encuentra partir de una fecha
    /// </summary>
    internal long GetTimeStep(DateTime? timestamp = null) => GetTimeStep(GetUnixTime(GetCorrectedTime(timestamp)));

    /// <summary>
    ///     Convierte una fecha a tiempo Unix
    /// </summary>
    private long GetUnixTime(DateTime timestamp) => (long) (timestamp - Epoch).TotalSeconds;

    /// <summary>
    ///     Aplica el factor de corrección a una fecha
    /// </summary>
    private DateTime GetCorrectedTime(DateTime? timestamp = null) => (timestamp ?? DateTime.UtcNow) - TimeCorrectionFactor;

    /// <summary>
    ///     Inicio del intervalo de tiempo
    /// </summary>
    public DateTime WindowStart(DateTime? timestamp = null)
    {
        DateTime corrected = GetCorrectedTime(timestamp);

            return corrected.AddTicks(-(corrected.Ticks - EpochTicks) % (TicksToSeconds * IntervalSeconds));
    }

    /// <summary>
    ///     Segundos restantes en la ventana de tiempo actual
    /// </summary>
    public int GetRemainingSeconds(DateTime? timestamp = null)
    {
        return IntervalSeconds - ((int) ((GetCorrectedTime(timestamp).Ticks - EpochTicks) / TicksToSeconds)) % IntervalSeconds;
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