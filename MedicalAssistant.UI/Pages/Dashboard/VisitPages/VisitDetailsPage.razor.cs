using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Pages.Dashboard.VisitPages;

public partial class VisitDetailsPage
{
	private bool _isVisitLoading = true;
	private bool _areNotificationLoading = true;

	[Parameter]
	public Guid VisitId { get; set; }

	[Inject]
	public IVisitService VisitService { get; set; }

	private VisitDto Visit { get; set; }


	protected override async Task OnInitializedAsync()
	{
		_isVisitLoading = true;
		var response = await VisitService.Get(VisitId);
		if (response.IsSuccess)
		{
			Visit = response.Value!;
			_isVisitLoading = false;
		}
	}
}
