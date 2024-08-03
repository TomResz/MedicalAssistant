using MedicalAssist.UI.Shared.Resources;
using MedicalAssist.UI.Shared.Services.Abstraction;
using MedicalAssist.UI.Shared.Services.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace MedicalAssist.UI.Pages.AuthCallbacks;

public partial class AuthGoogleCallback
{
	[Inject]
	public IUserAuthService AuthService { get; set; }

	[Inject]
	public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

	[Inject]
	public NavigationManager NavigationManager { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

	[Parameter]
	[SupplyParameterFromQuery(Name = "code")]
	public string? Code { get; set; }

	[Parameter]
	[SupplyParameterFromQuery(Name = "error")]
	public string? Error { get; set; }

	protected override async Task OnParametersSetAsync()
	{
		if (Code is not null)
		{
			var response = await AuthService.SignInByGoogle(Code);
			if (response.IsSuccess)
			{
				await (AuthenticationStateProvider as MedicalAssistAuthenticationStateProvider)!.AuthenticateAsync(response.Value!);
				NavigationManager.NavigateTo("/");
			}
			else
			{
				Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
				NavigationManager.NavigateTo("/login");
			}
		}

		NavigationManager.NavigateTo("/login");
	}
}
