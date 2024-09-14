namespace MedicalAssistant.Infrastructure.Security;
public interface IAESService
{
	string EncryptStringToBase64(string plainText);
	string DecryptStringFromBase64(string base64);
}
