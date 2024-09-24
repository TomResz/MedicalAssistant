using System.Globalization;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface ILanguageManager
{
    Task ChangeLanguage(CultureInfo? cultureInfo);
    Task<string?> GetCurrentLanguage();
}
