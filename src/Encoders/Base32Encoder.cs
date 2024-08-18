namespace Bau.Libraries.OneTimePassword.Encoders;

/// <summary>
///		Codificación de una cadena a Base32 basado en RFC 3548/4648 que utiliza el alfabeto "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567"
/// </summary>
internal class Base32Encoder
{
	// Constantes privadas
	private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
	private const char Padding = '=';
	private const int EncodedBitSize = 5;
	private const int ByteBitSize = 8;

	/// <summary>
	///		Decodifica una cadena
	/// </summary>
	internal byte[] Decode(string encoded)
	{
		string input = encoded.ToUpper().Trim().TrimEnd(Padding);
		byte[] buffer = new byte[input.Length * EncodedBitSize / ByteBitSize];
		byte active = 0;
		int bitsRemaining = ByteBitSize, mask = 0, index = 0;

			// Decodifica los diferentes caracteres
			foreach (char chr in input)
			{
				int encodedValue = Alphabet.IndexOf(chr);

					if (encodedValue < 0)
						throw new ArgumentException($"Illegal character ({chr}) found in encoded data string");
					else if (bitsRemaining > EncodedBitSize)
					{
						mask = encodedValue << (bitsRemaining - EncodedBitSize);
						active = (byte) (active | mask);
						bitsRemaining -= EncodedBitSize;
					}
					else
					{
						mask = encodedValue >> (EncodedBitSize - bitsRemaining);
						active = (byte) (active | mask);
						buffer[index++] = active;
						active = (byte) (encodedValue << (ByteBitSize - EncodedBitSize + bitsRemaining));
						bitsRemaining += ByteBitSize - EncodedBitSize;
					}
			}
			// Devuelve la cadena convertida
			return buffer;
	}

	/// <summary>
	///		Codifica una cadena
	/// </summary>
	internal string Encode(string plain) => Encode(System.Text.Encoding.UTF8.GetBytes(plain));

	/// <summary>
	///		Codifica un array de bytes
	/// </summary>
	internal string Encode(byte[] plain)
	{
		int outputCharSize = (int) Math.Ceiling(plain.Length / (decimal) EncodedBitSize) * ByteBitSize;
		byte[] buffer = new byte[outputCharSize];
		byte activeValue = 0;
		int remainingBits = EncodedBitSize;
		int index = 0;

			// Recorre cada byte y lo codifica en 5 bits
			foreach (byte activeByte in plain)
			{
				activeValue = (byte) (activeValue | (activeByte >> (ByteBitSize - remainingBits)));
				buffer[index++] = (byte) Alphabet[activeValue];
				if (remainingBits <= ByteBitSize - EncodedBitSize)
				{
					activeValue = (byte) (activeByte >> (ByteBitSize - EncodedBitSize - remainingBits) & 31);
					buffer[index++] = (byte) Alphabet[activeValue];
					remainingBits += EncodedBitSize;
				}
				remainingBits -= ByteBitSize - EncodedBitSize;
				activeValue = (byte) ((activeByte << remainingBits) & 31);
			}
			// Añade el último carácter
			if (index != outputCharSize)
				buffer[index++] = (byte) Alphabet[activeValue];
			// Añade los caracteres de relleno para cumplir con la especificación que indica que la cadena deben formarlas bloques de 40 bits
			while (index < outputCharSize)
				buffer[index++] = (byte)Padding;
			// Devuelve la cadena
			return System.Text.Encoding.ASCII.GetString(buffer);
	}
}