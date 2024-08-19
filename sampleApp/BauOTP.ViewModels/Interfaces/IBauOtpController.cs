namespace Bau.Libraries.BauOTP.ViewModels.Interfaces;

/// <summary>
///		Interface para los controladores de la aplicación
/// </summary>
public interface IBauOtpController
{
	/// <summary>
	///		Abre la vista de generación de claves OTP
	/// </summary>
	bool OpenOtpView(Generators.GeneratorViewModel generatorViewModel);

	/// <summary>
	///		Muestra una ventana de mensaje
	/// </summary>
	void ShowMessage(string message);

	/// <summary>
	///		Muestra una ventana de interrogación sí / no
	/// </summary>
	bool ShowQuestion(string message);
}
