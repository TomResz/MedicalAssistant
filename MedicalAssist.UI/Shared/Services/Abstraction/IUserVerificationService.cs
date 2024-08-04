
using System.Threading.Tasks;

namespace MedicalAssist.UI.Shared.Services.Abstraction;

public interface IUserVerificationService
{
    Task<Response.Base.Response> VerifyAccount(string code);
    Task<Response.Base.Response> RegenerateCode(string email);
}