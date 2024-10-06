using MedicalAssistant.UI.Models.Settings;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Globalization;
using static MedicalAssistant.UI.Components.AppBar.LanguageSwitchBtn;

namespace MedicalAssistant.UI.Components.Settings;

public partial class SettingsDialog
{
	[Inject]
	public ISettingsService SettingsService { get; set; }

	[Inject]
	public NavigationManager Navigation { get; set; }
	[Inject]
	public ILanguageManager LanguageManager { get; set; }
	[Parameter]
	public string? LanguageSwitchClass { get; set; } = null;

	[CascadingParameter]
	public MudDialogInstance MudDialog { get; set; }

	private bool _loading = false;	
	private bool _isLanguageChanged = false;

	private CultureInfo? _culture = null;
	private CultureInfo? _notificationCulture = null;
	
	private SettingsViewModel _viewModel =new();
	private Language? NotificationLanguage { get; set; } = null;


	protected override async Task OnInitializedAsync()
	{
		var response = await SettingsService.Get();
        if (response.IsSuccess)
        {
			_viewModel = response.Value!;
			NotificationLanguage = response.Value!.NotificationLanguage is "pl-PL" ? Language.Polish : Language.English;
			StateHasChanged();
        }
    }

	public void Cancel() => MudDialog.Cancel();

	private async Task SaveChanges()
	{
		_viewModel.NotificationLanguage = _notificationCulture!.Name is "pl-Pl" ? "pl-PL" : "en-US"; 
		_loading = true;	
		var response = await SettingsService.Update(_viewModel);
		if (response.IsFailure) 
		{
			MudDialog.Close();
		}
		_loading = false;
	}

	private async Task LanguageChanged(CultureInfo culture)
	{
		_culture = culture;
		_isLanguageChanged = !_isLanguageChanged;
		await LanguageManager.ChangeLanguage(_culture);
		Navigation.NavigateTo(Navigation.Uri, true);
	}

	private void NotificationLanguageChanged(CultureInfo culture)
	{
		_notificationCulture = culture;
	}
}
