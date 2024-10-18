using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.Medication;

public partial class AddMedicationRecommendationDialog
{
	private MudForm _form;

	private VisitDto? _visitDto = null;

	private MedicationViewModel _viewModel = new();
	private MedicationViewModelValidator _validator = new();

	[CascadingParameter]
	public MudDialogInstance DialogInstance { get; set; }

	[Parameter]
	public DateRange? DateRange { get; set; } = null;

	[Inject]
	public IMedicationService MedicationService { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

	protected override void OnParametersSet()
	{
		if (DateRange is not null)
		{
			_viewModel.DateRange = DateRange;
		}
	}

	private async Task VisitPick(VisitDto visitDto)
	{
		_visitDto = visitDto;
		await InvokeAsync(StateHasChanged);
	}

	private async Task Submit()
	{
		await _form.Validate();
		if (!_form.IsValid)
		{
			return;
		}

		var request = _viewModel.ToInsertRequest();
		Response<AddMedicationResponse> response = await MedicationService.Add(request);

		if (response.IsSuccess)
		{
			var responseValue = response.Value!;

			var dto = new MedicationDto
			{
				Id = responseValue.Id,
				Visit = responseValue.Visit,
				EndDate = request.EndDate,
				StartDate = request.StartDate,
				TimeOfDay = request.TimeOfDay,
				ExtraNote = request.ExtraNote,
				Name = request.MedicineName,
				Quantity = request.Quantity,
			};

			DialogInstance.Close(dto);
			Snackbar.Add(Translations.RecommendationAdded, Severity.Success);
			return;
		}
		else
		{
			Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
		}
	}

	public void Cancel() => DialogInstance.Cancel();

	private async Task Delete()
	{
		_visitDto = null;
		await InvokeAsync(StateHasChanged);
	}
}


