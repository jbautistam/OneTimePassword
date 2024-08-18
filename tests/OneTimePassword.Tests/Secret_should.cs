using FluentAssertions;
using Bau.Libraries.OneTimePassword;

namespace OneTimePassword.Tests;

/// <summary>
///		Pruebas de la clase <see cref="Secret"/>
/// </summary>
public class Secret_should
{
	/// <summary>
	///		Codificar una clave
	/// </summary>
	[Theory]
	[InlineData("abc", Secret.Encoding.Plain, "abc")]
	[InlineData("abc", Secret.Encoding.Base32, "MFRGG===")]
	[InlineData("abc", Secret.Encoding.Base64, "YWJj")]
	public void Encode(string key, Secret.Encoding encoding, string expected)
	{
		Secret secret = new(key, encoding);

			secret.Encode().Should().Be(expected);
	}

	/// <summary>
	///		Decodificar una clave
	/// </summary>
	[Theory]
	[InlineData("abc", Secret.Encoding.Plain, "abc")]
	[InlineData("MFRGG===", Secret.Encoding.Base32, "abc")]
	[InlineData("YWJj", Secret.Encoding.Base64, "abc")]
	public void Decode(string key, Secret.Encoding encoding, string expected)
	{
		Secret secret = new(key, encoding);

			System.Text.Encoding.UTF8.GetString(secret.Decode()).Should().Be(expected);
	}
}
