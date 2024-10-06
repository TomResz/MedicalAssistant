using MedicalAssistant.UI.Shared.Resources;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.Settings;

public partial class SettingBtn
{
	[Parameter]
	public string? Class { get; set; }

	[Inject]
	public IDialogService DialogService { get; set; }


	private readonly DialogOptions options = new() 
	{ 
		CloseOnEscapeKey = true, 
		FullWidth = true 
	};

	protected override async Task OnInitializedAsync()
	{
				
	}


	public void ShowSettingDialog()
	{
		var dialog = DialogService.Show<SettingsDialog>(Translations.Settings, options);
	}
}
