using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface INotificationService
{
	Task<Response<List<NotificationModel>>> GetUnread();
	Task<Response.Base.Response> MarkAsRead(List<Guid> ids);
	Task<Response<PagedList<NotificationModel>>> GetPage(int page, int pageSize);
}