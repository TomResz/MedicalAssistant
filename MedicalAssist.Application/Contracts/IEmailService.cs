using MedicalAssist.Domain.Enums;

namespace MedicalAssist.Application.Contracts;
public interface IEmailService
{
	Task SendMailWithRegenerateVerificationCode(string email,string newVerificationCode, Languages langueage);
	Task SendMailWithVerificationCode(string email,string verificationCode,Languages langueage);
	Task SendMailWithChangePasswordCode(string email,string code, Languages langueage);
}