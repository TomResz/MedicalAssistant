using MedicalAssistant.UI.Models.RegenareteCode;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Auth;

public partial class RegenerateVerificationCode
{
	[Inject]
	public IUserVerificationService UserVerificationService { get; set; }

    [Inject]
    public ISnackbar Snackbar { get; set; }

    private MudForm _form;
	private bool _btnPressed = false;
	private readonly RegenerateCodeModel _model = new();
	private readonly RegenerateCodeModelValidator _validator = new();

	private bool _emailSent = false;

	private async Task Regenerate()
	{
		await _form.Validate();
		if (_form.IsValid)
		{
			_btnPressed = true;
			var response = await UserVerificationService.RegenerateCode(_model.Email);

			if (response.IsSuccess)
			{
				_emailSent = true;
			}
			else
			{
				var error = response.ErrorDetails!.Type switch
				{
					"AccountIsAlreadyVerified" => Translations.AccountIsVerified,
					"UserNotFound" => Translations.UserNotFoundByGivenEmail,
					_ => Translations.SomethingWentWrong
				};
				Snackbar.Add(error,Severity.Error);	
			}
		}
		_btnPressed = false;
	}
}
