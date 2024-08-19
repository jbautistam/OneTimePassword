using Bau.Libraries.OneTimePassword;

namespace Bau.Libraries.BauOTP.ViewModels.Models;

/// <summary>
///		Modelo con los datos de los servidores definidos para OTP
/// </summary>
public class OtpPropertyModel
{
	/// <summary>
	///		Nombre del servidor
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	///		Clave del servidor
	/// </summary>
	public string? Key { get; set; }

	/// <summary>
	///		Codificación de la clave
	/// </summary>
	public Secret.Encoding Encoding { get; set; } = Secret.Encoding.Plain;

	/// <summary>
	///		Algoritmo
	/// </summary>
	public BaseTokenGenerator.HashAlgorithm HashAlgorithm { get; set; } = BaseTokenGenerator.HashAlgorithm.Sha1;

	/// <summary>
	///		Dígitos
	/// </summary>
	public int Digits { get; set; } = 6;

	/// <summary>
	///		Contador
	/// </summary>
	public long Counter { get; set; } = 1;

	/// <summary>
	///		Intervalo (en segundos)
	/// </summary>
	public int Interval { get; set; } = 30;
}
