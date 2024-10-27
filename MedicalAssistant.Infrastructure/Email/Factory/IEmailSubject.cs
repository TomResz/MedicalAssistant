namespace MedicalAssistant.Infrastructure.Email.Factory;
internal interface IEmailSubject
{
	string PasswordChange { get; }	
	string Verification { get; }	
	string CodeRegeneration { get; }	
	string VisitNotification { get; }
	string MedicationRecommendation { get; }
}
