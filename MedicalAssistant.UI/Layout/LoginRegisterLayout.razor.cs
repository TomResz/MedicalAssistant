using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;


namespace MedicalAssistant.UI.Layout;

public partial class LoginRegisterLayout
{
	[CascadingParameter] BaseLayout BaseLayout { get; set; }
	private bool isAuthenticated;

	private const string DarkModeIcon = Icons.Material.Filled.Nightlight;
	private const string LightModeIcon = Icons.Material.Filled.Brightness4;

	[Inject] NavigationManager Navigation { get; set; }
	[Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
	[Inject] IAuthorizationService AuthorizationService { get; set; }

	[Inject] ILanguageManager LanguageManager { get; set; }

	protected override async Task OnInitializedAsync()
	{
		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		isAuthenticated = authState.User.Identity!.IsAuthenticated;
		var isNotActive = await AuthorizationService.AuthorizeAsync(authState.User, null, "IsNotActive");
		if (isAuthenticated)
		{
			if (isNotActive == AuthorizationResult.Success())
			{
				return;
			}
			Navigation.NavigateTo("/");
		}
	}

	private async Task ChangeLanguage(CultureInfo culture)
	{
		await LanguageManager.ChangeLanguage(culture);
		Navigation.NavigateTo(Navigation.Uri, true);
	}
}
