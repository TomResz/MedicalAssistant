using MedicalAssistant.UI.Models.PasswordChange;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.User;

public partial class ChangePasswordDialog
{
	[Inject]
	public ISnackbar Snackbar { get; set; }


	[Inject]
	public IUserPasswordChangeService PasswordService { get; set; }
	
	
	[CascadingParameter]
	public MudDialogInstance MudDialog { get; set; }



	private MudForm _form;
	private readonly ChangePasswordModel _model = new();
	private readonly ChangePasswordModelValidator _validator = new();



	private bool _btnPressed = false;

	private InputType passwordInputType = InputType.Password;
	private string passwordIcon = Icons.Material.Filled.VisibilityOff;

	private InputType confirmedPasswordInputType = InputType.Password;
	private string confirmedPasswordIcon = Icons.Material.Filled.VisibilityOff;

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

	public async Task SubmitForm()
	{
		await _form.Validate();

		if (!_form.IsValid)
		{
			return;
		}
		_btnPressed = true;
		var response = await PasswordService.ChangePassword(_model);

		if(response.IsSuccess)
		{
			Snackbar.Add(Translations.PasswordChanged, Severity.Success);
			MudDialog.Close();
			return;
		}

		var error = response.ErrorDetails!.Type switch
		{
			"InvalidNewPassword" => Translations.InvalidNewPassword,
			_ => Translations.SomethingWentWrong
		};
		Snackbar.Add(error, Severity.Error);
		_btnPressed = false;
	}


	public void Cancel() => MudDialog.Cancel();
}
