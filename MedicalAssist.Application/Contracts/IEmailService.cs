namespace MedicalAssist.Application.Contracts;
public interface IEmailService
{
	Task SendMailWithVerificationCode(string email,string verificationCode);
}
