﻿using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssist.UI.Components.Settings;

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

	public void ShowSettingDialog()
	{
		var dialog = DialogService.Show<SettingsDialog>("Settings", options);
	}
}
