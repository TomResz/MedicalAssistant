using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Infrastructure.Email.Factory;

namespace MedicalAssistant.Infrastructure.Email;
internal sealed class EmailService : IEmailService
{
	private readonly IEmailSender _emailSender;
	private readonly EmailRoutes _routes;
	public EmailService(IEmailSender emailSender, EmailRoutes routes)
	{
		_emailSender = emailSender;
		_routes = routes;
	}

	public async Task SendMailWithChangePasswordCode(string email, string code, Languages language)
	{
		var body = EmailBodyFactory.Create(language).PasswordChange(_routes.PasswordChange, code);
		var subject = EmailSubjectFactory.Create(language).PasswordChange;	

		await _emailSender.SendEmailAsync(email, subject, body);
	}
	public async Task SendMailWithRegenerateVerificationCode(string email, string newVerificationCode, Languages language)
	{
		var body = EmailBodyFactory.Create(language).GetRegeneratedVerificationCodeHtml(_routes.RegeneratedVerificationCodeRoute, newVerificationCode);
		var subject = EmailSubjectFactory.Create(language).CodeRegeneration;

		await _emailSender.SendEmailAsync(email, subject, body);
	}

	public async Task SendMailWithVerificationCode(string email, string verificationCode, Languages language)
	{
		var body = EmailBodyFactory.Create(language).GetVerificationCodeHtml(_routes.VerificationCodeRoute, verificationCode);
		var subject = EmailSubjectFactory.Create(language).Verification;

		await _emailSender.SendEmailAsync(email, subject, body);
	}
}
