namespace Bau.Libraries.OneTimePassword.TimeTools;

/// <summary>
///     Ventana de verificación: frames o intervalos que se consideran válidos (por ejemplo, si queremos que sean válidos desde uno antes al frame actual
/// hasta uno después lo inicializaríamos con 1 - 1
/// </summary>
public class VerificationWindow
{
    public VerificationWindow(int previous = 1, int future = 1)
    {
        Previous = previous;
        Future = future;
    }

    /// <summary>
    ///     Enumera todos los candidatos a validar
    /// </summary>
    public IEnumerable<long> ValidationCandidates(long initialFrame)
    {
        // Devuelve el frame actual
        yield return initialFrame;
        // Devuelve los frames anteriores
        for (int index = 1; index <= Previous; index++)
        {
            long actual = initialFrame - index;

                // siempre y cuando sean válidos
                if (actual >= 0)
                    yield return actual;
        }
        // Devuelve los frames siguientes
        for (int index = 1; index <= Future; index++)
            yield return initialFrame + index;
    }

    /// <summary>
    ///     Número de frames anteriores
    /// </summary>
    public int Previous { get; }

    /// <summary>
    ///     Número de frames posteriores
    /// </summary>
    public int Future { get; }
}
