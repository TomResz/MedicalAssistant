using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Shared.Extensions;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Pages.Dashboard.Medication;

public partial class CurrentMedications
{
	private bool _loading = true;
	private bool _secondLoading = true;

	[Inject]
	public IMedicationService MedicationService { get; set; }

	[Inject]
	public ILocalTimeProvider LocalTimeProvider { get; set; }

	private List<MedicationWithDayDto> _currentWeek = [];
	private List<MedicationWithDayDto> _nextWeek = [];

	private DateTime CurrentMonday;
	private DateTime CurrentSunday;
	
	private DateTime NextMonday;
	private DateTime NextSunday;
	protected override async Task OnInitializedAsync()
	{
		var date = await LocalTimeProvider.CurrentDate();

		CurrentMonday = date.StartOfWeek();
		CurrentSunday = CurrentMonday.AddDays(6);

		NextMonday = CurrentMonday.AddDays(7);
		NextSunday = NextMonday.AddDays(6);
		
		Response<List<MedicationWithDayDto>> response = await MedicationService.Week(date);

		if(response.IsSuccess)
		{
			_currentWeek = response.Value!;
			_loading = false;
		}

		Response<List<MedicationWithDayDto>> secondResponse = await MedicationService.Week(date.AddDays(7));

		if (secondResponse.IsSuccess)
		{
			_nextWeek = secondResponse.Value!;
			_secondLoading = false;
		}

	}
}
