namespace MedicalAssist.Infrastructure.Email.Factory;
internal interface IEmailBody
{
	string GetVerificationCodeHtml(string route, string verificationCode);
	string GetRegeneratedVerificationCodeHtml(string route, string newVerificationCode);
	string PasswordChange(string route, string code);
}
