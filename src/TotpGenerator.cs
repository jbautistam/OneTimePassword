using Bau.Libraries.OneTimePassword.TimeTools;

namespace Bau.Libraries.OneTimePassword;

/// <summary>
///		Generador de token basado en tiempo (TOTP) (https://datatracker.ietf.org/doc/html/rfc6238)
/// </summary>
public class TotpGenerator : BaseTokenGenerator
{
    public TotpGenerator(string secret, HashAlgorithm hashAlgorithm, int digits) : base(secret, hashAlgorithm, digits) {}

    /// <summary>
    ///     Genera el token a partir de un un timestamp
    /// </summary>
    public string Compute(long timestamp) => ComputeToken(TimeManager.GetTimeStep(timestamp));

    /// <summary>
    ///     Genera el token a partir de una fecha
    /// </summary>
    public string Compute(DateTime? timestamp = null) => ComputeToken(TimeManager.GetTimeStep(timestamp));

    /// <summary>
    ///     Verifica un token con respecto al valor calculado
    /// </summary>
    public (bool verified, long matchedStep) Verify(string token, int previous = 1, int future = 1) => Verify(DateTime.UtcNow, token, previous, future);

    /// <summary>
    ///     Verifica un token con respecto al valor calculado
    /// </summary>
    public (bool verified, long matchedStep) Verify(DateTime timestamp, string token, int previous = 1, int future = 1)
    {
        long initialStep = TimeManager.GetTimeStep(timestamp);

            // Genera y valida cadan uno de los frames de la ventana        
            foreach (long frame in ValidationCandidates(initialStep, previous, future))
                if (ValuesEqual(ComputeToken(frame), token))
                    return (true, frame);
            // Devuelve el valor que indica que la validación es incorrecta
            return (false, -1);
    }

    /// <summary>
    ///     Enumera todos los candidatos a validar
    /// </summary>
    private IEnumerable<long> ValidationCandidates(long initialStep, int previous, int future)
    {
        // Devuelve el frame actual
        yield return initialStep;
        // Devuelve los frames anteriores
        for (int index = 1; index <= previous; index++)
        {
            long actual = initialStep - index;

                // siempre y cuando sean válidos
                if (actual >= 0)
                    yield return actual;
        }
        // Devuelve los frames siguientes
        for (int index = 1; index <= future; index++)
            yield return initialStep + index;
    }

    /// <summary>
    ///     Controlador para cálculos de horas
    /// </summary>
    public TotpTimeManager TimeManager { get; } = new();
}