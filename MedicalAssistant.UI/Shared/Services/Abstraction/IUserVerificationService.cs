
using System.Threading.Tasks;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IUserVerificationService
{
    Task<Response.Base.Response> VerifyAccount(string code);
    Task<Response.Base.Response> RegenerateCode(string email);
}