using MedicalAssistant.UI.Components.Medication;
using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Dashboard.Medication;

public partial class DailyMedicationsPage
{
	[Inject]
	public IMedicationService MedicationService { get; set; }

	[Inject]
	public ILocalTimeProvider LocalTimeProvider { get; set; }


	private List<MedicationDto> afternoonRecommendations = [];
	private List<MedicationDto> eveningRecommendations = [];
	private List<MedicationDto> nightRecommendations = [];
	private List<MedicationDto> morningRecommendations = [];

	private bool _isMedicationLoading = true;

	[Inject]
	public ISnackbar Snackbar { get; set; }

	[Inject]
	public NavigationManager NavigationManager { get; set; }

	protected override async Task OnInitializedAsync()
	{
		var date = await LocalTimeProvider.CurrentDate();
		var response = await MedicationService.GetByDate(date);

		if (response.IsSuccess)
		{
			List<MedicationDto> values = response.Value!;

			morningRecommendations = values.Where(x => x.TimeOfDay.Contains(MedicationMappers.Morning)).ToList();
			afternoonRecommendations = values.Where(x => x.TimeOfDay.Contains(MedicationMappers.Afternoon)).ToList();
			eveningRecommendations = values.Where(x => x.TimeOfDay.Contains(MedicationMappers.Evening)).ToList();
			nightRecommendations = values.Where(x => x.TimeOfDay.Contains(MedicationMappers.Night)).ToList();

			_isMedicationLoading = false;
		}
		else
		{
			NavigationManager.NavigateTo("/");
			Snackbar.Add(Translations.SomethingWentWrong,Severity.Error);
		}
	}
}
