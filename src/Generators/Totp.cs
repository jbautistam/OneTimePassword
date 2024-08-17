using Bau.Libraries.OneTimePassword.TimeTools;

namespace Bau.Libraries.OneTimePassword.Generators;

/// <summary>
///		Generador de token basado en tiempo (TOTP) (https://datatracker.ietf.org/doc/html/rfc6238)
/// </summary>
public class Totp : BaseOtp
{
    public Totp(string secret, OneTimePasswordGenerator.HashAlgorithm hashAlgorithm, int digits) : base(secret, hashAlgorithm, digits) {}

    /// <summary>
    /// The number of ticks as Measured at Midnight Jan 1st 1970;
    /// </summary>
    private const long UnicEpocTicks = 621355968000000000L;

    /// <summary>
    /// A divisor for converting ticks to seconds
    /// </summary>
    private const long TicksToSeconds = 10000000L;

    /// <summary>
    ///     Genera el token a partir de una fecha
    /// </summary>
    public string ComputeTotp(DateTime? timestamp = null) => Compute(CalculateTimeStepFromTimestamp(GetCorrectedTime(timestamp)));

    /// <summary>
    ///     Calcula el intervalo en el que está a partir de la hora
    /// </summary>
    private long CalculateTimeStepFromTimestamp(DateTime timestamp) => TimeCycle.GetTimeStep(timestamp, Interval);

    /// <summary>
    ///     Aplica el factor de corrección a la fecha de sistema
    /// </summary>
    private DateTime GetCorrectedTime(DateTime? reference = null) => (reference ?? DateTime.UtcNow) - TimeCorrectionFactor;

    /// <summary>
    /// Verify a value that has been provided with the calculated value.
    /// </summary>
    public bool VerifyTotp(string totp, out long timeStepMatched, VerificationWindow? window = null) =>
        VerifyTotpForSpecificTime(GetCorrectedTime(), totp, window, out timeStepMatched);

    /// <summary>
    /// Verify a value that has been provided with the calculated value
    /// </summary>
    public bool VerifyTotp(DateTime timestamp, string totp, out long timeStepMatched, VerificationWindow? window = null) =>
        VerifyTotpForSpecificTime(GetCorrectedTime(timestamp), totp, window, out timeStepMatched);

    /// <summary>
    /// Remaining seconds in current window
    /// </summary>
    public int RemainingSeconds(DateTime? timestamp = null) => RemainingSecondsForSpecificTime(GetCorrectedTime(timestamp));

    /// <summary>
    /// Start of the current window
    /// </summary>
    public DateTime WindowStart(DateTime? timestamp = null) => WindowStartForSpecificTime(GetCorrectedTime(timestamp));

    private bool VerifyTotpForSpecificTime(DateTime timestamp, string totp, VerificationWindow? window, out long timeStepMatched)
    {
        var initialStep = CalculateTimeStepFromTimestamp(timestamp);
        return Verify(initialStep, totp, out timeStepMatched, window);
    }

    /// <summary>
    ///     Verifica un token
    /// </summary>
    protected bool Verify(long initialStep, string valueToVerify, out long matchedStep, VerificationWindow? window)
    {
        window = window ?? new VerificationWindow();
        
        foreach (long frame in window.ValidationCandidates(initialStep))
        {
            string comparisonValue = Compute(frame);

            if (ValuesEqual(comparisonValue, valueToVerify))
            {
                matchedStep = frame;
                return true;
            }
        }

        matchedStep = 0;
        return false;
    }

    private int RemainingSecondsForSpecificTime(DateTime timestamp) =>
        Interval - (int)(((timestamp.Ticks - UnicEpocTicks) / TicksToSeconds) % Interval);

    private DateTime WindowStartForSpecificTime(DateTime timestamp) =>
        timestamp.AddTicks(-(timestamp.Ticks - UnicEpocTicks) % (TicksToSeconds * Interval));

    /// <summary>
    ///     Segundos de la ventana de tiempo de generación de tokens
    /// </summary>
    public int Interval { get; set; } = 30;

    /// <summary>
    ///     Factor de corrección de la hora del sistema para compensar si está fuera de sincronización
    /// </summary>
    public TimeSpan TimeCorrectionFactor { get; set; } = TimeSpan.FromSeconds(0);
}
