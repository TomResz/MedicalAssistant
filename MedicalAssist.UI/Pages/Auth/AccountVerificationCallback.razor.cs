using MedicalAssist.UI.Shared.Resources;
using MedicalAssist.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssist.UI.Pages.Auth;

public partial class AccountVerificationCallback
{
	[Parameter]
	[SupplyParameterFromQuery(Name = "code")]
	public string? Code { get; set; } = null;

	[Inject]
	public IUserVerificationService UserVerificationService { get; set; }

    [Inject]
    public ISnackbar Snackbar { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    private bool _loading = true;

	private VerificationStatus _verificationStatus;

	protected override async Task OnParametersSetAsync()
	{
		if (Code is not null)
		{
			var response = await UserVerificationService.VerifyAccount(Code);

			if(response.IsSuccess)
			{
				_verificationStatus = VerificationStatus.Success;
			}
			else
			{
				_verificationStatus = response.ErrorDetails!.Type switch
				{
					"InactiveVerificationCode" => VerificationStatus.CodeExpired,
					_ => VerificationStatus.InvalidCode
				};
			}

			_loading = false;
		}
	}

	private void InvalidVerificationCodePrompt()
	{
		Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
		Navigation.NavigateTo("/login");
	}
}
