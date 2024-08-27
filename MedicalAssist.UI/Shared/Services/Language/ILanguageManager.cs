using System.Globalization;

namespace MedicalAssist.UI.Shared.Services.Language;

public interface ILanguageManager
{
	Task ChangeLanguage(CultureInfo? cultureInfo);
	Task<string?> GetCurrentLanguage();	
}
