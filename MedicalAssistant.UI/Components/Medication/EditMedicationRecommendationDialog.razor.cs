using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.Medication;

public partial class EditMedicationRecommendationDialog
{
	private MudForm _form;

	private VisitDto? _visitDto = null;

	private MedicationViewModel _viewModel = new();
	private MedicationViewModelValidator _validator = new();

	[CascadingParameter]
	public MudDialogInstance DialogInstance { get; set; }

	[Parameter]
	public MedicationDto Medication { get; set; }

    [Parameter]
    public Guid Id { get; set; }

    [Inject]
	public IMedicationService MedicationService { get; set; }

	[Inject]
	public IVisitService VisitService { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }


	protected override void OnParametersSet()
	{
		if (Medication is null)
		{
			DialogInstance.Close();
			return;
		}

		_visitDto = Medication.Visit;
		_viewModel = Medication.ToViewModel();


	}

	private async Task Submit()
	{
		await _form.Validate();
		if (!_form.IsValid)
		{
			return;
		}
		var request = _viewModel.ToUpdateRequest(Id);

		Response<VisitDto?> response = await MedicationService.Update(request);

		if (response.IsSuccess)
		{
			_visitDto = response.Value;

			var dto = _viewModel.ToDto(_visitDto,Id);
			DialogInstance.Close(dto);
			Snackbar.Add(Translations.MedicationEdited,Severity.Success);
			return;
		}
		else
		{
			Snackbar.Add(Translations.SomethingWentWrong,Severity.Error);
		}

	}
	private async Task VisitPick(VisitDto visitDto)
	{
		_visitDto = visitDto;
		await InvokeAsync(StateHasChanged);
	}
	public void Cancel() => DialogInstance.Cancel();

	private async Task Delete()
	{
		_visitDto = null;
		_viewModel.VisitId = null;
		await InvokeAsync(StateHasChanged);
	}

}
