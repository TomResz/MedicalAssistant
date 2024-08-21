using MedicalAssist.UI.Models.Visits;
using MedicalAssist.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;

namespace MedicalAssist.UI.Pages.Dashboard;

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
