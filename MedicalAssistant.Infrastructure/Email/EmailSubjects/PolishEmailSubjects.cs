using MedicalAssistant.Infrastructure.Email.Factory;

namespace MedicalAssistant.Infrastructure.Email.EmailSubjects;
internal class PolishEmailSubjects : IEmailSubject
{
	public string PasswordChange => "Asystent Medyczny - Zmiana Hasła";

	public string Verification => "Asystent Medyczny - Weryfikacja Konta";

	public string CodeRegeneration => "Asystent Medyczny - Weryfikacja Konta";

	public string VisitNotification => "Asystent Medyczny - Powiadomienie o Wizycie";

	public string MedicationRecommendation => "Asystent Medyczny - Powiadomienie o Zaleceniach";
}
