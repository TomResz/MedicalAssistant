using MedicalAssist.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using System.Net;

namespace MedicalAssist.Infrastructure.Email;
internal static class Extensions
{
	private const string emailSettingOptions = "smtp";
	private const string routeOptions = "urls";
	internal static IServiceCollection AddEmailServices(this IServiceCollection services,IConfiguration configuration)
	{
		var emailClientSettings = configuration.GetOptions<EmailClientSettings>(emailSettingOptions);
		var routes = configuration.GetOptions<EmailRoutes>(routeOptions);

		var smtpClient = new SmtpClient(emailClientSettings.Host)
		{
			Port = emailClientSettings.Port,
			Credentials = new NetworkCredential(emailClientSettings.Email, emailClientSettings.Password),
			EnableSsl = true,
		};
		services
			.AddSingleton(routes)
			.AddSingleton(smtpClient)
			.AddSingleton(emailClientSettings)
			.AddSingleton<IEmailSender, EmailSender>()
			.AddSingleton<IEmailService, EmailService>();
		return services;
	}
}
