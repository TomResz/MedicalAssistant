using MedicalAssistant.Application.Security;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;

namespace MedicalAssistant.Infrastructure.Security;
internal sealed class EmailCodeManager : IEmailCodeManager
{
	private readonly IAESService _AESService;
	public EmailCodeManager(IAESService aESService)
	{
		_AESService = aESService;
	}
	public bool Decode(string code, out string email)
	{
		email = string.Empty;

		try
		{
			var decrypted = _AESService.DecryptStringFromBase64(code);
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
		=> _AESService.EncryptStringToBase64(email);
}

