using MedicalAssistant.Application.Dto;
using MedicalAssistant.Domain.Enums;

namespace MedicalAssistant.Application.Contracts;
public interface IEmailService
{
	Task SendMailWithRegenerateVerificationCode(string email,string newVerificationCode, Languages langueage);
	Task SendMailWithVerificationCode(string email,string verificationCode,Languages langueage);
	Task SendMailWithChangePasswordCode(string email,string code, Languages langueage);
	Task SendMailWithVisitNotification(string email,VisitDto visit, Languages language);
	Task SendMailWithMedicationRecommendation(string email,MedicationRecommendationDto medicationRecommendation,Languages langueage);
}