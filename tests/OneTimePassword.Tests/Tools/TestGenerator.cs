using Bau.Libraries.OneTimePassword;

namespace OneTimePassword.Tests.Tools;

/// <summary>
///		Generador de casos de prueba
/// </summary>
public class TestGenerator
{
	// Variables privadas
	private Random _rnd = new();

	/// <summary>
	///		Crea diferentes teorías para <see cref="TotpGenerator_Should"/> y <see cref="HotpGenerator_Should"/>: 
	///		escribe las cadenas de InlineData sobre la consola de depuración
	/// </summary>
	[Fact(Skip = "Use when generate inlinedata for tests")]
	public void Generate_Theories_Totp()
	{
		System.Text.StringBuilder builder = new();

			// Genera las teorías
			for (int type = 0; type < 2; type++)
			{
				// Añade la cabecera adecuada
				if (type == 0)
					builder.AppendLine("===== TOTP =======");
				else
					builder.AppendLine("===== HOTP =======");
				// Añade los datos para InlineData
				for (int index = 0; index < 3; index++)
				{
					builder.AppendLine(GenerateOtpTheory(GenerateKey(8), Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, _rnd.Next(1_000_000), _rnd.Next(6, 8), type == 0));
					builder.AppendLine(GenerateOtpTheory(GenerateKey(8), Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha256, _rnd.Next(1_000_000), _rnd.Next(6, 8), type == 0));
					builder.AppendLine(GenerateOtpTheory(GenerateKey(8), Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha512, _rnd.Next(1_000_000), _rnd.Next(6, 8), type == 0));
					builder.AppendLine(GenerateOtpTheory(GenerateKey(8), Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha1, _rnd.Next(1_000_000), _rnd.Next(6, 8), type == 0));
					builder.AppendLine(GenerateOtpTheory(GenerateKey(8), Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha256, _rnd.Next(1_000_000), _rnd.Next(6, 8), type == 0));
					builder.AppendLine(GenerateOtpTheory(GenerateKey(8), Secret.Encoding.Base32, BaseTokenGenerator.HashAlgorithm.Sha512, _rnd.Next(1_000_000), _rnd.Next(6, 8), type == 0));
					builder.AppendLine(GenerateOtpTheory(GenerateKey(8), Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha1, _rnd.Next(1_000_000), _rnd.Next(6, 8), type == 0));
					builder.AppendLine(GenerateOtpTheory(GenerateKey(8), Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha256, _rnd.Next(1_000_000), _rnd.Next(6, 8), type == 0));
					builder.AppendLine(GenerateOtpTheory(GenerateKey(8), Secret.Encoding.Base64, BaseTokenGenerator.HashAlgorithm.Sha512, _rnd.Next(1_000_000), _rnd.Next(6, 8), type == 0));
				}
			}
			// Muestra las teorías para poder generar las teorías
			System.Diagnostics.Debug.WriteLine(builder.ToString());
	}

	/// <summary>
	///		Genera una teoría para <see cref="TotpGenerator"/> o <see cref="HotpGenerator"/>
	/// </summary>
	private string GenerateOtpTheory(string key, Secret.Encoding encoding, BaseTokenGenerator.HashAlgorithm algorithm, long timestamp, int digits, bool isTotp)
	{
		string theory = $"[InlineData(\"{key}\", Secret.Encoding.{encoding.ToString()}, BaseTokenGenerator.HashAlgorithm.{algorithm.ToString()}, {timestamp.ToString()}, ";
		TotpGenerator generator = new(key, encoding, algorithm, digits);

			// Añade la clave
			if (isTotp)
				theory += $"\"{new TotpGenerator(key, encoding, algorithm, digits).Compute(timestamp)}\"";
			else
				theory += $"\"{new HotpGenerator(key, encoding, algorithm, digits).Compute(timestamp)}\"";
			// Añade el final de la teoría
			theory += ")]";
			// y la devuelve
			return theory;
	}

	/// <summary>
	///		Genera una clave
	/// </summary>
	private string GenerateKey(int length)
	{
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567"; // alfabeto reducido correcto para Base32
		string key = string.Empty;

			// Genera la clave
			for (int index = 0; index < length;index++)
				key += alphabet[_rnd.Next(alphabet.Length)];
			// Devuelve la clave
			return key;
	}
}
