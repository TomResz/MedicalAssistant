using MedicalAssistant.UI.Shared.Services.Abstraction;
using MedicalAssistant.UI.Shared.Services.Attachment;
using MedicalAssistant.UI.Shared.Services.HubToken;
using MedicalAssistant.UI.Shared.Services.Language;
using MedicalAssistant.UI.Shared.Services.Medication;
using MedicalAssistant.UI.Shared.Services.MedicationNotifications;
using MedicalAssistant.UI.Shared.Services.Notifications;
using MedicalAssistant.UI.Shared.Services.RefreshToken;
using MedicalAssistant.UI.Shared.Services.Settings;
using MedicalAssistant.UI.Shared.Services.Time;
using MedicalAssistant.UI.Shared.Services.User;
using MedicalAssistant.UI.Shared.Services.Verification;
using MedicalAssistant.UI.Shared.Services.Visits;

namespace MedicalAssistant.UI.Shared.Services;

public static class Extensions
{
	public static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddScoped<INotificationService, MedicalAssistant.UI.Shared.Services.Notifications.NotificationService>();
		services.AddScoped<ILanguageManager, LanguageManager>();
		services.AddScoped<IRefreshTokenService, RefreshTokenService>();
		services.AddScoped<IUserAuthService, UserAuthService>();
		services.AddScoped<IVisitService, VisitService>();
		services.AddScoped<IHubTokenService, HubTokenService>();
		services.AddScoped<IUserVerificationService, UserVerificationService>();
		services.AddScoped<IUserDataService, UserDataService>();
		services.AddScoped<IUserPasswordChangeService, UserPasswordChangeService>();
		services.AddScoped<IVisitNotificationService, VisitNotificationService>();
		services.AddScoped<ILocalTimeProvider, LocalTimeProvider>();
		services.AddScoped<IAttachmentService, AttachmentService>();
		services.AddScoped<ISettingsService, SettingsService>();
		services.AddScoped<IMedicationService, MedicationService>();
		services.AddScoped<IMedicationNotificationService,MedicationNotificationService>();

		return services;
	}
}
