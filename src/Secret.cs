namespace Bau.Libraries.OneTimePassword;

/// <summary>
///		Clase con los datos de un secreto
/// </summary>
public class Secret
{
	/// <summary>
	///		Codificación de la clave
	/// </summary>
	public enum Encoding
	{
		/// <summary>Texto plano</summary>
		Plain,
		/// <summary>Texto en Base32</summary>
		Base32,
		/// <summary>Texto en Base64</summary>
		Base64
	}

	public Secret(string key, Encoding mode = Encoding.Plain)
	{
		Key = key;
		Mode = mode;
	}

	/// <summary>
	///		Decodifica la clave a un array de bytes
	/// </summary>
	public byte[] Decode()
	{
		return Mode switch
				{
					Encoding.Base32 => new Encoders.Base32Encoder().Decode(Key),
					Encoding.Base64 => new Encoders.Base64Encoder().DecodeToBytes(Key),
					_ => System.Text.Encoding.UTF8.GetBytes(Key)
				}; 
	}

	/// <summary>
	///		Codifica la clave
	/// </summary>
	public string Encode()
	{
		return Mode switch
				{
					Encoding.Base32 => new Encoders.Base32Encoder().Encode(Key),
					Encoding.Base64 => new Encoders.Base64Encoder().Encode(Key),
					_ => Key
				};
	}

	/// <summary>
	///		Clave
	/// </summary>
	public string Key { get; } 
	
	/// <summary>
	///		Modo de codificación
	/// </summary>
	public Encoding Mode { get; }
}
