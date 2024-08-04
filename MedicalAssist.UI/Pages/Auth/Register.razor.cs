using MedicalAssist.UI.Models.Register;
using MedicalAssist.UI.Shared.Resources;
using MedicalAssist.UI.Shared.Services.Abstraction;
using MedicalAssist.UI.Shared.Services.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace MedicalAssist.UI.Pages.Auth;

public partial class Register
{
	[Inject]
	public IUserAuthService UserService { get; set; }

	[Inject]
	public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

	[Inject]
	public NavigationManager Navigation { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

	private MudForm _form;
	private readonly RegisterUserModel user = new();
	private readonly RegisterUserModelValidator _validator = new();

	private InputType passwordInputType = InputType.Password;
	private string passwordIcon = Icons.Material.Filled.VisibilityOff;

	private InputType confirmedPasswordInputType = InputType.Password;
	private string confirmedPasswordIcon = Icons.Material.Filled.VisibilityOff;

	private bool _btnPressed = false;


	private async Task RegisterAsync()
	{
		await _form.Validate();
		if (_form.IsValid)
		{
			_btnPressed = true;
			var response = await UserService.SignUp(new()
			{
				Email = user.Email,
				FullName = user.FullName,
				Password = user.Password
			});

			if (response.IsSuccess)
			{
				user.FullName = string.Empty;
				user.Email = string.Empty;
				user.ConfirmedPassword = string.Empty;
				user.Password = string.Empty;
				Snackbar.Add(Translations.RegisterPageAccountCreated, Severity.Success);
				Snackbar.Add(Translations.RegisterPageEmailSent, Severity.Success);
			}
			else
			{
				string error = response.ErrorDetails!.Type switch
				{
					AuthErrors.EmailInUse => Translations.CreateAccountEmailInUsePrompt,
					_ => Translations.SomethingWentWrong
				};
				Snackbar.Add(error, Severity.Error);
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

	private void ToggleConfirmedPasswordVisibility()
	{
		if (confirmedPasswordInputType == InputType.Password)
		{
			confirmedPasswordInputType = InputType.Text;
			confirmedPasswordIcon = Icons.Material.Filled.Visibility;
		}
		else
		{
			confirmedPasswordInputType = InputType.Password;
			confirmedPasswordIcon = Icons.Material.Filled.VisibilityOff;
		}
	}
}
