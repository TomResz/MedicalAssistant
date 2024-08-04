using MedicalAssist.UI.Models.PasswordChange;
using MedicalAssist.UI.Shared.Resources;
using MedicalAssist.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssist.UI.Pages.Auth;

public partial class ForgotPassword
{
    [Inject]
    public ISnackbar Snackbar { get; set; }

    [Inject]
    public IUserPasswordChangeService PasswordChangeService { get; set; }


    private MudForm _form;

	private readonly ForgotPasswordModel _model = new();
	private readonly ForgotPasswordModelValidator _validator = new();

	private bool _btnPressed = false;

	private async Task SendMail()
	{
		await _form.Validate();

		if(_form.IsValid)
		{
			_btnPressed = true;
			var response = await PasswordChangeService.SendMailToChangePassword(_model);
			_model.Email = string.Empty;

			if (response.IsSuccess)
			{
				Snackbar.Add(Translations.PasswordChangedPageEmailSent, Severity.Success);
			}
			else
			{
				Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
			}
		}
		_btnPressed = false;
	}
}
