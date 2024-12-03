using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Infrastructure.Email;

namespace MedicalAssistant.API.Tests.Abstractions;

public class TestEmailSender : IEmailSender
{
    public Task SendEmailAsync(string receiver, string subject, string bodyHtml)
    {
        return Task.CompletedTask;
    }
}