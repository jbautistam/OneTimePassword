using System.Text;

namespace Bau.Libraries.OneTimePassword.Encoders;

/// <summary>
///		Codificador a base64
/// </summary>
internal class Base64Encoder
{
	/// <summary>
	///		Codifica a Base64
	/// </summary>
	internal string Encode(string plain) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));

	/// <summary>
	///		Decodifica desde Base64
	/// </summary>	
	internal string Decode(string encoded) => Encoding.UTF8.GetString(Convert.FromBase64String(encoded));

	/// <summary>
	///		Decodifica desde Base64
	/// </summary>	
	internal byte[] DecodeToBytes(string encoded) => Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(Convert.FromBase64String(encoded)));
}
