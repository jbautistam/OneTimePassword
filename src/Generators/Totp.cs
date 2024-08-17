using Bau.Libraries.OneTimePassword.TimeTools;

namespace Bau.Libraries.OneTimePassword.Generators;

/// <summary>
///		Generador de token basado en tiempo (TOTP) (https://datatracker.ietf.org/doc/html/rfc6238)
/// </summary>
public class Totp : BaseOtp
{
    /// <summary>
    /// The number of ticks as Measured at Midnight Jan 1st 1970;
    /// </summary>
    private const long UnicEpocTicks = 621355968000000000L;

    /// <summary>
    /// A divisor for converting ticks to seconds
    /// </summary>
    private const long TicksToSeconds = 10000000L;

    public Totp(string secret, OneTimePasswordGenerator.HashAlgorithm hashAlgorithm, int digits) : base(secret, hashAlgorithm, digits) {}

    /// <summary>
    ///     Genera el token a partir de una fecha
    /// </summary>
    public string Compute(DateTime? timestamp = null) => ComputeToken(CalculateTimeStepFromTimestamp(GetCorrectedTime(timestamp)));

    /// <summary>
    ///     Calcula el intervalo en el que está a partir de la hora
    /// </summary>
    private long CalculateTimeStepFromTimestamp(DateTime timestamp) => TimeCycle.GetTimeStep(timestamp, Interval);

    /// <summary>
    ///     Aplica el factor de corrección a una fecha
    /// </summary>
    private DateTime GetCorrectedTime(DateTime? reference = null) => (reference ?? DateTime.UtcNow) - TimeCorrectionFactor;

    /// <summary>
    ///     Segundos restantes en la ventana de tiempo actual
    /// </summary>
    public int RemainingSeconds(DateTime? timestamp = null) => Interval - ((int) ((GetCorrectedTime(timestamp).Ticks - UnicEpocTicks) / TicksToSeconds)) % Interval;

    /// <summary>
    ///     Inicio del intervalo de tiempo
    /// </summary>
    public DateTime WindowStart(DateTime? timestamp = null)
    {
        DateTime corrected = GetCorrectedTime(timestamp);

            return corrected.AddTicks(-(corrected.Ticks - UnicEpocTicks) % (TicksToSeconds * Interval));
    }

    /// <summary>
    /// Verify a value that has been provided with the calculated value.
    /// </summary>
    public bool VerifyTotp(string totp, out long timeStepMatched, VerificationWindow? window = null) => VerifyTotpForSpecificTime(GetCorrectedTime(), totp, window, out timeStepMatched);

    /// <summary>
    /// Verify a value that has been provided with the calculated value
    /// </summary>
    public bool VerifyTotp(DateTime timestamp, string totp, out long timeStepMatched, VerificationWindow? window = null) => VerifyTotpForSpecificTime(GetCorrectedTime(timestamp), totp, window, out timeStepMatched);

    private bool VerifyTotpForSpecificTime(DateTime timestamp, string totp, VerificationWindow? window, out long timeStepMatched)
    {
        var initialStep = CalculateTimeStepFromTimestamp(timestamp);
        return Verify(initialStep, totp, out timeStepMatched, window);
    }

    /// <summary>
    ///     Verifica un token
    /// </summary>
    private bool Verify(long initialStep, string valueToVerify, out long matchedStep, VerificationWindow? window)
    {
        window = window ?? new VerificationWindow();
        
        foreach (long frame in window.ValidationCandidates(initialStep))
        {
            string comparisonValue = ComputeToken(frame);

            if (ValuesEqual(comparisonValue, valueToVerify))
            {
                matchedStep = frame;
                return true;
            }
        }

        matchedStep = 0;
        return false;
    }

    /// <summary>
    ///     Segundos de la ventana de tiempo de generación de tokens
    /// </summary>
    public int Interval { get; set; } = 30;

    /// <summary>
    ///     Factor de corrección de la hora del sistema para compensar si está fuera de sincronización
    /// </summary>
    public TimeSpan TimeCorrectionFactor { get; set; } = TimeSpan.FromSeconds(0);
}
