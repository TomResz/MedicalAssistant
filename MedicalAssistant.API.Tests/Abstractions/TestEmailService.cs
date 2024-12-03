using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Domain.Enums;

namespace MedicalAssistant.API.Tests.Abstractions;

public class TestEmailService : IEmailService
{
    public string LastSentCode { get; private set; }
    public Task SendMailWithRegenerateVerificationCode(string email, string newVerificationCode, Languages langueage)
    {
        LastSentCode = newVerificationCode;
        return Task.CompletedTask;
    }

    public Task SendMailWithVerificationCode(string email, string verificationCode, Languages langueage)
    {
        LastSentCode = verificationCode;
        return Task.CompletedTask;
    }

    public Task SendMailWithChangePasswordCode(string email, string code, Languages langueage)
    {
        LastSentCode = code;
        return Task.CompletedTask;
    }

    public Task SendMailWithVisitNotification(string email, VisitDto visit, Languages language)
    {
        return Task.CompletedTask;
    }

    public Task SendMailWithMedicationRecommendation(
        string email, MedicationRecommendationDto medicationRecommendation, Languages langueage)
    {
        return Task.CompletedTask;
    }
}