using MedicalAssistant.UI.Models.Login;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using MedicalAssistant.UI.Shared.Services.Auth;
using MedicalAssistant.UI.Shared.Services.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Auth;

public partial class Login
{
	[Inject]
	public IUserAuthService UserService { get; set; }
	
	[Inject]
	public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
	
	[Inject]
	public NavigationManager Navigation {  get; set; }
	
	[Inject]
	public ISnackbar Snackbar { get; set; }	

	private MudForm _form;
	private readonly LoginModel _user = new();
	private InputType passwordInputType = InputType.Password;
	private string passwordIcon = Icons.Material.Filled.VisibilityOff;

	private bool _btnPressed = false;
	private readonly LoginModelValidator _validator = new();

	private async Task LoginAsync()
	{
		await _form.Validate();
		if (_form.IsValid)
		{
			_btnPressed = true;
			var response = await UserService.SignIn(_user);

			if (response.IsSuccess)
			{
				await (AuthenticationStateProvider as MedicalAssistAuthenticationStateProvider)!.AuthenticateAsync(response.Value!);
				Navigation.NavigateTo("/");
			}
			else
			{
				string error = response.ErrorDetails!.Type switch
				{
					AuthErrors.InvalidLoginProvider => Translations.InvalidLoginProviderError,
					AuthErrors.InvalidSignInCredentials => Translations.InvalidSignInCredentialsError,
					AuthErrors.UnverifiedUser => Translations.UnverifiedUserError,
					_ => Translations.SomethingWentWrong
				};

				Snackbar.Add(error, MudBlazor.Severity.Error);
				Console.WriteLine(error);
			}
			_btnPressed = false;
		}
	}

	private void TogglePasswordVisibility()
	{
		if (passwordInputType == InputType.Password)
		{
			passwordInputType = InputType.Text;
			passwordIcon = Icons.Material.Filled.Visibility;
		}
		else
		{
			passwordInputType = InputType.Password;
			passwordIcon = Icons.Material.Filled.VisibilityOff;
		}
	}
}
