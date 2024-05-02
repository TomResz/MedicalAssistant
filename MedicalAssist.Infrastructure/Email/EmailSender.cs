using System.Net.Mail;
using System.Net.Mime;

namespace MedicalAssist.Infrastructure.Email;
internal sealed class EmailSender : IEmailSender
{
	private readonly SmtpClient _client;
	private readonly EmailClientSettings _settings;

	private const string _imagePath = "..\\Images\\background.jpg";

	public EmailSender(SmtpClient client, EmailClientSettings settings)
	{
		_client = client;
		_settings = settings;
	}

	public async Task SendEmailAsync(string receiver, string subject, string bodyHtml)
	{
		var mailMessage = new MailMessage
		{
			From = new MailAddress(_settings.Email),
			Subject = subject,
			Body = bodyHtml,
			IsBodyHtml = true,
		};

		mailMessage.AlternateViews.Add(AddBackgroundView(_imagePath, bodyHtml));
		mailMessage.To.Add(receiver);

		await _client.SendMailAsync(mailMessage);
	}
	private static AlternateView AddBackgroundView(string imagePath, string body)
	{
		LinkedResource imageResource = new LinkedResource(imagePath);
		imageResource.ContentId = Guid.NewGuid().ToString();

		string html = $@"
		<html>
		    <head>
		        <style>
		            .container {{
		                padding: 40px;
		                border-radius: 20px;
		                text-align: center;
		            }}
		            .content {{
		                background-color: rgba(255, 255, 255, 1);
		                padding: 20px;
		                border-radius: 10px;
		            }}
		        </style>
		    </head>
		    <body>
		        <table class='container' style='background-image: url(""cid:{imageResource.ContentId}""); background-size: cover; background-position: center; width: 100%;heigth: 100%'>
		            <tr>
		                <td class='content'>
		                    {body}
		                </td>
		            </tr>
		        </table>
		    </body>
		</html>";

		AlternateView alternateView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
		alternateView.LinkedResources.Add(imageResource);

		return alternateView;
	}
}
