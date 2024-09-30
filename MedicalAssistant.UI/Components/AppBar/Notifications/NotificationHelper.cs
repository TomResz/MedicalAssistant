using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace MedicalAssistant.UI.Components.AppBar.Notifications;

public static class NotificationHelper
{
	public static string ToType(NotificationModel model)
	{
		if (model.Type == NotificationTypes.VisitNotification)
		{
			return Translations.NotificationOfVisit;
		}
		return string.Empty;
	}

	public static string ToNotificationDescription(NotificationModel notification)
	{
		if (notification.Type == NotificationTypes.VisitNotification)
		{
			var visitObj = JsonSerializer.Deserialize<VisitDto>(notification.ContentJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
			return string.Format(Translations.VisitDateCommunicate,visitObj.Date.ToString());
		}
		return string.Empty;
	}

	public static void GoToNotificationDetails(NotificationModel notification, NavigationManager navigationManager)
	{
		if (notification.Type == NotificationTypes.VisitNotification)
		{
			VisitDto visitObj = JsonSerializer.Deserialize<VisitDto>(notification.ContentJson, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			})!;
			navigationManager.NavigateTo($"visit/{visitObj.Id}");
		}
	}
}
