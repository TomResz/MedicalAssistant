﻿namespace MedicalAssistant.Infrastructure.Email;
public interface IEmailSender
{
	Task SendEmailAsync(string receiver, string subject, string bodyHtml);
}
