using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Extensions;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Pages.Dashboard.VisitPages;

public partial class CurrentVisits
{
	private bool _loading = true;
	private bool _secondLoading = true;
	private IReadOnlyList<VisitDto> _currentVisits;
	private IReadOnlyList<VisitDto> _nextWeekVisits;

	[Inject]
	public IVisitService VisitService { get; set; }

	[Inject]
	public ILocalTimeProvider TimeProvider { get; set; }

	private DateTime CurrentMonday = DateTime.Now;
	private DateTime CurrentSunday = DateTime.Now;
	
	private DateTime NextMonday = DateTime.Now;
	private DateTime NextSunday = DateTime.Now;
	
	protected override async Task OnInitializedAsync()
	{
		var currentWeekDate = (await TimeProvider.CurrentDate()).StartOfWeek();
		var nextWeekDate = currentWeekDate.AddDays(7);
		
		CurrentMonday = currentWeekDate.Date;
		CurrentSunday = CurrentMonday.AddDays(6);

		NextMonday = CurrentMonday.AddDays(7);
		NextSunday = CurrentSunday.AddDays(7);
		
		Response<List<VisitDto>> response = await VisitService.GetByWeek(currentWeekDate);
		Response<List<VisitDto>> nextWeekResponse = await VisitService.GetByWeek(nextWeekDate);

		if (response.IsSuccess)
		{
			_currentVisits = [.. response.Value!];
			_loading = false;
		}
		if (nextWeekResponse.IsSuccess)
		{
			_nextWeekVisits = [.. nextWeekResponse.Value!];
			_secondLoading = false;
		}
	}
}
