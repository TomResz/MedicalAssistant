using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace MedicalAssist.Infrastructure.Security;
internal sealed class AESService(IOptions<AESOptions> options) : IAESService
{
	private readonly byte[] _key = Encoding.UTF8.GetBytes(options.Value.Key);
	private readonly byte[] _iv = Encoding.UTF8.GetBytes(options.Value.IV);

	public string DecryptStringFromBase64(string base64)
	{
		var cipherText = Convert.FromBase64String(base64);
		using (var aesAlg = Aes.Create())
		{
			aesAlg.Key = _key;
			aesAlg.IV = _iv;

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

	public string EncryptStringToBase64(string plainText)
	{
		using (var aesAlg = Aes.Create())
		{
			aesAlg.Key = _key;
			aesAlg.IV = _iv;

			var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

			using (var msEncrypt = new MemoryStream())
			{
				using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
				{
					using (var swEncrypt = new StreamWriter(csEncrypt))
					{
						swEncrypt.Write(plainText);
					}
					var byteArray = msEncrypt.ToArray();

					return Convert.ToBase64String(byteArray);
				}
			}
		}
	}
}
