using MedicalAssistant.UI.Models.Notifications;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IVisitNotificationService
{
    Task<Response.Base.Response<List<VisitNotificationDto>>> Get(Guid visitId);
}
