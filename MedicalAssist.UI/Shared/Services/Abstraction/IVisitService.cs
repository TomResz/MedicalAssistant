using MedicalAssist.UI.Models.Visits;
using MedicalAssist.UI.Shared.Response.Base;

namespace MedicalAssist.UI.Shared.Services.Abstraction;
public interface IVisitService
{
    Task<Response<List<VisitModel>>> GetAllVisits();
}