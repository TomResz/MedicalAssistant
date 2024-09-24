using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Dashboard.VisitPages;

public partial class VisitDetailsPage
{
	private bool _isVisitLoading = true;
	private bool _areNotificationLoading = true;

	[Parameter]
	public Guid VisitId { get; set; }

	[Inject]
	public IVisitService VisitService { get; set; }

	[Inject]
	public IVisitNotificationService VisitNotificationService { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

    private VisitDto Visit { get; set; }
	private List<VisitNotificationDto> _visitNotifications { get; set; }

	protected override async Task OnInitializedAsync()
	{
		_isVisitLoading = true;
		var response = await VisitService.Get(VisitId);
		if (response.IsSuccess)
		{
			Visit = response.Value!;
			_isVisitLoading = false;
		}
		else
		{
			Snackbar.Add(Translations.VisitNotFound, Severity.Error);
			Navigation.NavigateTo("/visits");
			return;
		}

		var notificationResponse = await VisitNotificationService.Get(VisitId);
		if (notificationResponse.IsSuccess)
		{
			_visitNotifications = notificationResponse.Value!;
			_areNotificationLoading = false;
		}
	}
}
