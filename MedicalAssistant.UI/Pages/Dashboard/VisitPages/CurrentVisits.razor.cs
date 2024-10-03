using MedicalAssistant.UI.Models.Visits;
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

	protected override async Task OnInitializedAsync()
	{
		var currentWeekDate = await TimeProvider.CurrentDate();
		var nextWeekDate = currentWeekDate.AddDays(7);

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
