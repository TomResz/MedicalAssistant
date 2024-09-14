using MedicalAssistant.UI.Models.PasswordChange;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Auth;

public partial class PasswordChangeCallback
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "code")]
    public string? Code { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public ISnackbar Snackbar { get; set; }

    [Inject]
    public IUserPasswordChangeService UserPasswordChangeService { get; set; }


    private bool _loading = true;
	private readonly ChangePasswordModel _model = new();
	private readonly ChangePasswordModelValidator _validator = new();

	private InputType passwordInputType = InputType.Password;
	private string passwordIcon = Icons.Material.Filled.VisibilityOff;

	private InputType confirmedPasswordInputType = InputType.Password;
	private string confirmedPasswordIcon = Icons.Material.Filled.VisibilityOff;


	private MudForm _form;
	private bool _btnPressed = false;
	protected override async Task OnParametersSetAsync()
	{
		if (Code is null || (await UserPasswordChangeService.CheckCode(Code)).IsFailure)
		{
			Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
			NavigationManager.NavigateTo("/login");
		}
        _loading = false;
	}

	public async Task ChangePassword()
	{
		await _form.Validate();
		if(!_form.IsValid || Code is null)
		{
			return;
		}

		var response = await UserPasswordChangeService.ChangePasswordByEmail(Code, _model.Password);

		if (response.IsSuccess)
		{
			Snackbar.Add(Translations.PasswordChanged, Severity.Success);
			NavigationManager.NavigateTo("/login");
			return;
		}
		Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
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
