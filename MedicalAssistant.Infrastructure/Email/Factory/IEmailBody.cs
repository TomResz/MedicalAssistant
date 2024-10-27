using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Infrastructure.Email.Factory;
internal interface IEmailBody
{
	string GetVerificationCodeHtml(string route, string verificationCode);
	string GetRegeneratedVerificationCodeHtml(string route, string newVerificationCode);
	string PasswordChange(string route, string code);
	string VisitNotification(VisitDto visitDto, string route);
	string MedicationRecommendation(string route, MedicationRecommendationDto dto);
}
