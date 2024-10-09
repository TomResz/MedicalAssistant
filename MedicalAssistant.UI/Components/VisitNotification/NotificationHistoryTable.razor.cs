using MedicalAssistant.UI.Components.AppBar.Notifications;
using MedicalAssistant.UI.Models.Notifications;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Components.VisitNotification;

public partial class NotificationHistoryTable
{
	[Inject] 
	public NavigationManager Navigation { get; set; }

    [Parameter] 
	public IReadOnlyList<NotificationModel> Notifications { get; set; }
	
	[Parameter] 
	public IReadOnlyDictionary<DateTime, string> FormattedDates { get; set; }
	
	private void ShowDetails(NotificationModel model)
		=> NotificationHelper.GoToNotificationDetails(model, Navigation);
}
