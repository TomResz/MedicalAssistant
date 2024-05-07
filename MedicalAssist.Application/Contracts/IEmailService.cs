namespace MedicalAssist.Application.Contracts;
public interface IEmailService
{
	Task SendMailWithRegenerateVerificationCode(string email,string newVerificationCode);
	Task SendMailWithVerificationCode(string email,string verificationCode);
	Task SendMailWithChangePasswordCode(string email,string code);
}