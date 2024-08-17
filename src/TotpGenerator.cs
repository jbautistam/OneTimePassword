using Bau.Libraries.OneTimePassword.TimeTools;

namespace Bau.Libraries.OneTimePassword;

/// <summary>
///		Generador de token basado en tiempo (TOTP) (https://datatracker.ietf.org/doc/html/rfc6238)
/// </summary>
public class TotpGenerator : BaseTokenGenerator
{
    /// <summary>
    /// The number of ticks as Measured at Midnight Jan 1st 1970;
    /// </summary>
    private const long UnicEpocTicks = 621355968000000000L;

    /// <summary>
    /// A divisor for converting ticks to seconds
    /// </summary>
    private const long TicksToSeconds = 10000000L;

    public TotpGenerator(string secret, HashAlgorithm hashAlgorithm, int digits) : base(secret, hashAlgorithm, digits) {}

    /// <summary>
    ///     Genera el token a partir de un un timestamp
    /// </summary>
    public string Compute(long timestamp) => Compute(UnixTime.GetDateTime(timestamp));

    /// <summary>
    ///     Genera el token a partir de una fecha
    /// </summary>
    public string Compute(DateTime? timestamp = null) => ComputeToken(TimeManager.GetTimeStep(timestamp));

    /// <summary>
    ///     Inicio del intervalo de tiempo
    /// </summary>
    public DateTime WindowStart(DateTime? timestamp = null)
    {
        DateTime corrected = TimeManager.GetCorrectedTime(timestamp);

            return corrected.AddTicks(-(corrected.Ticks - UnicEpocTicks) % (TicksToSeconds * TimeManager.IntervalSeconds));
    }

    /// <summary>
    /// Verify a value that has been provided with the calculated value.
    /// </summary>
    public bool VerifyTotp(string totp, out long timeStepMatched, VerificationWindow? window = null) => VerifyTotpForSpecificTime(DateTime.UtcNow, totp, window, out timeStepMatched);

    /// <summary>
    /// Verify a value that has been provided with the calculated value
    /// </summary>
    public bool VerifyTotp(DateTime timestamp, string totp, out long timeStepMatched, VerificationWindow? window = null) => VerifyTotpForSpecificTime(timestamp, totp, window, out timeStepMatched);

    private bool VerifyTotpForSpecificTime(DateTime timestamp, string totp, VerificationWindow? window, out long timeStepMatched)
    {
        long initialStep = TimeManager.GetTimeStep(timestamp);
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
    ///     Controlador para cálculos de horas
    /// </summary>
    public TotpTimeManager TimeManager { get; } = new();
}
