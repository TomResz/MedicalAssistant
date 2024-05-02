using MedicalAssist.Application.Contracts;

namespace MedicalAssist.Infrastructure.Email;
internal sealed class EmailService : IEmailService
{
	private readonly IEmailSender _emailSender;
	private readonly EmailRoutes _routes;
	public EmailService(IEmailSender emailSender, EmailRoutes routes)
	{
		_emailSender = emailSender;
		_routes = routes;
	}

	public async Task SendMailWithRegenerateVerificationCode(string email, string newVerificationCode)
		=> await _emailSender.SendEmailAsync("tomaszres@interia.pl", "Medical Assist - Verification Code", EmailHtmlBodies.GetRegeneratedVerificationCodeHtml(_routes.RegeneratedVerificationCodeRoute, newVerificationCode));


	public async Task SendMailWithVerificationCode(string email, string verificationCode) 
		=> await _emailSender.SendEmailAsync("tomaszres@interia.pl", "Medical Assist - Verification Code", EmailHtmlBodies.GetVerificationCodeHtml(_routes.VerificationCodeRoute,verificationCode));
}
