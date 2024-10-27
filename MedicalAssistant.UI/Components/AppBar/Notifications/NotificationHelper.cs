using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace MedicalAssistant.UI.Components.AppBar.Notifications;

public static class NotificationHelper
{
	private static JsonSerializerOptions _jsonSerializerOptions = new()
	{
		PropertyNameCaseInsensitive = true
	};

	public static string ToType(NotificationModel model)
	{
		if (model.Type == NotificationTypes.VisitNotification)
		{
			return Translations.NotificationOfVisit;
		}
		else if(model.Type == NotificationTypes.MedicationRecommendation)
		{

		}
		return string.Empty;
	}

	public static string ToNotificationDescription(NotificationModel notification)
	{
		if (notification.Type == NotificationTypes.VisitNotification)
		{
			var visitObj = JsonSerializer.Deserialize<VisitDto>(notification.ContentJson, _jsonSerializerOptions)!;
			return string.Format(Translations.VisitDateCommunicate, visitObj.Date.ToString());
		}
		else if (notification.Type == NotificationTypes.MedicationRecommendation)
		{
			var medicationDto = JsonSerializer.Deserialize<MedicationDto>(notification.ContentJson, _jsonSerializerOptions)!;
			return string.Format(Translations.MedicationNotificationCommunicate, medicationDto.Name);
		}
		return string.Empty;
	}

	public static void GoToNotificationDetails(NotificationModel notification, NavigationManager navigationManager)
	{
		if (notification.Type == NotificationTypes.VisitNotification)
		{
			VisitDto visitObj = JsonSerializer.Deserialize<VisitDto>(notification.ContentJson, _jsonSerializerOptions)!;
			navigationManager.NavigateTo($"visit/{visitObj.Id}");
		}
		else if(notification.Type == NotificationTypes.MedicationRecommendation)
		{
			var medicationDto = JsonSerializer.Deserialize<MedicationDto>(notification.ContentJson, _jsonSerializerOptions)!;
			navigationManager.NavigateTo($"medication/{medicationDto.Id}");
		}
	}
}
