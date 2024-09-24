using MedicalAssistant.UI.Shared.Services.Abstraction;
using MedicalAssistant.UI.Shared.Services.Auth;
using System.Globalization;

namespace MedicalAssistant.UI.Shared.Services.Language;

public class LanguageManager : ILanguageManager
{
	private readonly LocalStorageService _storageService;

	public LanguageManager(LocalStorageService storageService)
	{
		_storageService = storageService;
	}

	public async Task ChangeLanguage(CultureInfo? cultureInfo)
	{
		if(cultureInfo is not null)
		{
			await _storageService.SetValueAsync("Culture", cultureInfo.Name);
		}
	}

	public async Task<string?> GetCurrentLanguage()
		=> await _storageService.GetValueAsync("Culture");
}