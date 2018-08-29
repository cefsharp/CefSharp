#if NET40

using System;
using System.Text;

namespace CefSharp.SchemeHandler
{
	/// <summary>
	/// This class is copied verbatim (with a name change) from the .NET Framework 4.7.2 source-code
	/// https://referencesource.microsoft.com/#System/net/System/Net/HttpListenerRequest.cs,80a5cbf6a66fa610
	/// </summary>
	internal class UrlDecoderNet40
	{
		private int _bufferSize;

		// Accumulate characters in a special array
		private int _numChars;
		private char[] _charBuffer;

		// Accumulate bytes for decoding into characters in a special array
		private int _numBytes;
		private byte[] _byteBuffer;

		// Encoding to convert chars to bytes
		private Encoding _encoding;

		internal UrlDecoderNet40(int bufferSize, Encoding encoding)
		{
			_bufferSize = bufferSize;
			_encoding = encoding;

			_charBuffer = new char[bufferSize];
			// byte buffer created on demand
		}

		public static string UrlDecode(string encodedValue)
		{
			if (encodedValue == null)
				return null;

			return UrlDecodeInternal(encodedValue, Encoding.UTF8);
		}

		private static string UrlDecodeInternal(string value, Encoding encoding)
		{
			if (value == null)
			{
				return null;
			}

			int count = value.Length;
			UrlDecoderNet40 helper = new UrlDecoderNet40(count, encoding);

			// go through the string's chars collapsing %XX and
			// appending each char as char, with exception of %XX constructs
			// that are appended as bytes

			for (int pos = 0; pos < count; pos++)
			{
				char ch = value[pos];

				if (ch == '+')
				{
					ch = ' ';
				}
				else if (ch == '%' && pos < count - 2)
				{
					int h1 = HexToInt(value[pos + 1]);
					int h2 = HexToInt(value[pos + 2]);

					if (h1 >= 0 && h2 >= 0)
					{     // valid 2 hex chars
						byte b = (byte)((h1 << 4) | h2);
						pos += 2;

						// don't add as char
						helper.AddByte(b);
						continue;
					}
				}

				if ((ch & 0xFF80) == 0)
					helper.AddByte((byte)ch); // 7 bit have to go as bytes because of Unicode
				else
					helper.AddChar(ch);
			}

			return helper.GetString();
		}

		private static int HexToInt(char h)
		{
			return (h >= '0' && h <= '9') ? h - '0' :
			(h >= 'a' && h <= 'f') ? h - 'a' + 10 :
			(h >= 'A' && h <= 'F') ? h - 'A' + 10 :
			-1;
		}

		private void FlushBytes()
		{
			if (_numBytes > 0)
			{
				_numChars += _encoding.GetChars(_byteBuffer, 0, _numBytes, _charBuffer, _numChars);
				_numBytes = 0;
			}
		}

		internal void AddChar(char ch)
		{
			if (_numBytes > 0)
				FlushBytes();

			_charBuffer[_numChars++] = ch;
		}

		internal void AddByte(byte b)
		{
			if (_byteBuffer == null)
				_byteBuffer = new byte[_bufferSize];

			_byteBuffer[_numBytes++] = b;
		}

		internal String GetString()
		{
			if (_numBytes > 0)
				FlushBytes();

			if (_numChars > 0)
				return new String(_charBuffer, 0, _numChars);
			else
				return String.Empty;
		}
	}
}

#endif // NET40