using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Services.Notifications;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.VisitNotification;

public partial class UpcomingVisitNotificationsTable
{
	[Inject]
	public NavigationManager Navigation { get; set; }

	[Parameter]
	public IReadOnlyList<VisitNotificationWithDetailsModel> Notifications { get; set; }

	[Parameter]
	public IReadOnlyDictionary<DateTime, string> FormattedDates { get; set; }

	[Parameter]
	public EventCallback<Guid> OnDelete { get; set; }

	private MudMessageBox _mudMessageBox;
	public void GoToDetails(Guid visitId)
		=> Navigation.NavigateTo($"visit/{visitId}");

	public async Task Delete(Guid id)
	{
		var result = await _mudMessageBox.ShowAsync();

		if (result is null or false)
		{
			return;
		}
		await OnDelete.InvokeAsync(id);
	}
}
