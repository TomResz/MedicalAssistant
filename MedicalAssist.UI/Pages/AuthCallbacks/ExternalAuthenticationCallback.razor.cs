using MedicalAssist.UI.Shared.Resources;
using MedicalAssist.UI.Shared.Services.Abstraction;
using MedicalAssist.UI.Shared.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MedicalAssist.UI.Shared.Response;
using MedicalAssist.UI.Shared.Response.Base;

namespace MedicalAssist.UI.Pages.AuthCallbacks;

public partial class ExternalAuthenticationCallback
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
    public string Provider { get; set; }

    protected override async Task OnParametersSetAsync()
	{
		if (Code is not null)
		{
			var response = await GetResponse();
			if (response.IsSuccess)
			{
				await (AuthenticationStateProvider as MedicalAssistAuthenticationStateProvider)!.AuthenticateAsync(response.Value!);
				NavigationManager.NavigateTo("/");
			}
			else
			{
				Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
			}
		}
		NavigationManager.NavigateTo("/login");
	}

	private async Task<Response<SignInResponse>> GetResponse() 
	=> Provider switch
		{
			"google-callback" => await AuthService.SignInByGoogle(Code!),
			_ => await AuthService.SignInByFacebook(Code!),
		};
}
