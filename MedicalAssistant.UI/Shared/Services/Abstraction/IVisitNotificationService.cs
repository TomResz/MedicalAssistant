using MedicalAssistant.UI.Components.VisitNotification;
using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IVisitNotificationService
{
	Task<Response.Base.Response> ChangeDate(EditVisitNotificationModel model);
	Task<Response.Base.Response> Delete(Guid id);
	Task<Response.Base.Response<List<VisitNotificationDto>>> Get(Guid visitId);
	Task<Response.Base.Response<VisitNotificationDto>> Add(AddVisitNotificationModel model);
	string MatchErrors(BaseErrorDetails errorDetails);
	Task<Response<PagedList<VisitNotificationWithDetailsModel>>> GetPage(int page, int pageSize);
}
