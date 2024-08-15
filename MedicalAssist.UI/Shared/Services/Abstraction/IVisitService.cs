using MedicalAssist.UI.Models.Visits;
using MedicalAssist.UI.Shared.Response.Base;

namespace MedicalAssist.UI.Shared.Services.Abstraction;
public interface IVisitService
{
    Task<Response<List<VisitDto>>> GetAllVisits();
	Task<Response<VisitDto>> Add(CreateVisitVisitModel model);
	Task<Response.Base.Response> Delete(Guid visitId);
	Task<Response<VisitDto>> Edit(EditVisitModel editVisitModel);
}