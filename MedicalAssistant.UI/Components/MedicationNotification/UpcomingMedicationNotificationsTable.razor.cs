using MedicalAssistant.UI.Models.MedicationNotification;
using MedicalAssistant.UI.Models.Notifications;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.MedicationNotification;

public partial class UpcomingMedicationNotificationsTable
{
	[Inject]
	public NavigationManager Navigation { get; set; }

	[Parameter]
	public IReadOnlyList<MedicationNotificationPageContentDto> Notifications { get; set; }


	[Parameter]
	public EventCallback<Guid> OnDelete { get; set; }

	private MudMessageBox _mudMessageBox;
	public void GoToDetails(Guid medicationId)
		=> Navigation.NavigateTo($"medication/{medicationId}");

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
