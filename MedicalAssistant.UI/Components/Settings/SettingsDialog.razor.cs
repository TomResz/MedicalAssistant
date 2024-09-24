using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Globalization;

namespace MedicalAssistant.UI.Components.Settings;

public partial class SettingsDialog
{
	[Inject]
	public NavigationManager Navigation { get; set; }
	[Inject]
	public ILanguageManager LanguageManager { get; set; }
	[Parameter]
	public string? LanguageSwitchClass { get; set; } = null;

	[CascadingParameter]
	public MudDialogInstance MudDialog { get; set; }

	private bool _isLanguageChanged = false;

	private CultureInfo? _culture = null;

	public void Cancel() => MudDialog.Cancel();

	public bool IsNotificationsEnabled { get; set; }
	public bool AreVisitsEnabled { get; set; } = false;
	public bool AreEmailsEnabled { get; set; } = false;

	private async Task SaveChanges()
	{
		if (_isLanguageChanged)
		{
			await LanguageManager.ChangeLanguage(_culture);
			Navigation.NavigateTo(Navigation.Uri, true);
		}
		MudDialog.Close();
	}

	private void LanguageChanged(CultureInfo culture)
	{
		_culture = culture;
		_isLanguageChanged = !_isLanguageChanged;
	}
}
