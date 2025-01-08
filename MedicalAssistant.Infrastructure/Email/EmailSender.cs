using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net.Mime;

namespace MedicalAssistant.Infrastructure.Email;
internal sealed class EmailSender : IEmailSender
{
    private readonly SmtpClient _client;
    private readonly EmailClientSettings _settings;

    private static readonly string _imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "background.jpg");
    private readonly ILogger<EmailSender> _logger;
    public EmailSender(SmtpClient client, EmailClientSettings settings, ILogger<EmailSender> logger)
    {
        _client = client;
        _settings = settings;
        _logger = logger;
    }

    public async Task SendEmailAsync(string receiver, string subject, string bodyHtml)
    {
        var mailMessage = new MailMessage(_settings.Email, receiver)
        {
            Subject = subject,
            Body = bodyHtml,
            IsBodyHtml = true,
        };

        mailMessage.AlternateViews.Add(AddBackgroundView(_imagePath, bodyHtml));
        mailMessage.To.Add(receiver);

        try
        {
            await _client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "An error occurred while sending an email. Subject: {Subject}, To: {Recipient}, Exception Message: {ExceptionMessage}, StackTrace: {StackTrace}",
                mailMessage.Subject,
                string.Join(", ", mailMessage.To.Select(to => to.Address)),
                ex.Message,
                ex.StackTrace);
        }

    }
    private static AlternateView AddBackgroundView(string imagePath, string body)
    {
        LinkedResource imageResource = new(imagePath);
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
