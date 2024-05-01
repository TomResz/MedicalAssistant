using MedicalAssist.Application.Security;
using MedicalAssist.Domain.ValueObjects;
using System.Security.Cryptography;
using System.Text;

namespace MedicalAssist.Infrastructure.Security;
internal sealed class CodeVerification : ICodeVerification
{
	public string Generate(Date currentDate)
	{
		var uniqueId= Guid.NewGuid();
		string datePart = currentDate.Value.ToString("MMddHHmm");
		string guidPart = uniqueId.ToString("N");

		string combinedString = datePart + guidPart;

		string hashedCode;
		using (SHA256 sha256 = SHA256.Create())
		{
			byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedString));
			hashedCode = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
		}

		return hashedCode;
	}
}
