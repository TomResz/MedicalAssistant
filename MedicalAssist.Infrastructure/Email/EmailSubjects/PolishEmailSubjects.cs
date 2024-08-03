using MedicalAssist.Infrastructure.Email.Factory;

namespace MedicalAssist.Infrastructure.Email.EmailSubjects;
internal class PolishEmailSubjects : IEmailSubject
{
	public string PasswordChange => "Asystent Medyczny - Zmiana Hasła";

	public string Verification => "Asystent Medyczny - Weryfikacja Konta";

	public string CodeRegeneration => "Asystent Medyczny - Weryfikacja Konta";
}
