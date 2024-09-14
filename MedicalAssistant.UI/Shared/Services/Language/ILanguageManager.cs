using System.Globalization;

namespace MedicalAssistant.UI.Shared.Services.Language;

public interface ILanguageManager
{
	Task ChangeLanguage(CultureInfo? cultureInfo);
	Task<string?> GetCurrentLanguage();	
}
