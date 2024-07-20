using MedicalAssist.UI.Models.Login;
using MedicalAssist.UI.Shared.Response;

namespace MedicalAssist.UI.Shared.Services.Abstraction;

public interface IUserAuthService
{
    Task<SignInResponse> SignIn(LoginModel model);
    Task<SignInResponse> SignInByFacebook(string code);
    Task<SignInResponse> SignInByGoogle(string code);
}
