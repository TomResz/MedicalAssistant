using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Pages.Dashboard.VisitPages;

public partial class Visits
{
	[Inject]
	public IVisitService VisitService { get; set; }

	private bool _loading = true;

	private List<VisitDto> _visits = new();
	protected override async Task OnInitializedAsync()
	{
		var response = await VisitService.GetAllVisits();
		if (response.IsSuccess)
		{
			_visits = response.Value!;

		}
		_loading = false;
	}
}
