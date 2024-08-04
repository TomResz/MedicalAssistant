using MedicalAssist.Application.Security;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace MedicalAssist.Infrastructure.Security;
internal sealed class EmailCodeManager : IEmailCodeManager
{
	private readonly byte[] _key;
	private readonly byte[] _iv;

	public EmailCodeManager(IOptions<AESOptions> options)
	{
		_key = Encoding.UTF8.GetBytes(options.Value.Key); // 32 bytes, AES-256
		_iv = Encoding.UTF8.GetBytes(options.Value.IV); // 16 bytes
	}
	public bool Decode(string code, out string email)
	{
		email = string.Empty;

		try
		{
			var bytes = Convert.FromBase64String(code);
			var decrypted = DecryptStringFromBytes_Aes(bytes, _key, _iv);
			var parsedEmail = new Domain.ValueObjects.Email(decrypted);
			email = parsedEmail;
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public string Encode(string email)
	{
		var encrypted = EncryptStringToBytes_Aes(email, _key, _iv);
		return Convert.ToBase64String(encrypted);
	}

	private byte[] EncryptStringToBytes_Aes(string plainText, byte[] key, byte[] iv)
	{
		using (var aesAlg = Aes.Create())
		{
			aesAlg.Key = key;
			aesAlg.IV = iv;

			var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

			using (var msEncrypt = new MemoryStream())
			{
				using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
				{
					using (var swEncrypt = new StreamWriter(csEncrypt))
					{
						swEncrypt.Write(plainText);
					}
					return msEncrypt.ToArray();
				}
			}
		}
	}

	private string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] key, byte[] iv)
	{
		using (var aesAlg = Aes.Create())
		{
			aesAlg.Key = key;
			aesAlg.IV = iv;

			var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

			using (var msDecrypt = new MemoryStream(cipherText))
			{
				using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					using (var srDecrypt = new StreamReader(csDecrypt))
					{
						return srDecrypt.ReadToEnd();
					}
				}
			}
		}
	}
}

