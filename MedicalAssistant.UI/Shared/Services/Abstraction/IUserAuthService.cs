using MedicalAssistant.UI.Models.Login;
using MedicalAssistant.UI.Shared.Requests;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IUserAuthService
{
    Task<Response<SignInResponse>> SignIn(LoginModel model);
    Task<Response<SignInResponse>> SignInByFacebook(string code);
    Task<Response<SignInResponse>> SignInByGoogle(string code);

    Task<Response.Base.Response> SignUp(SignUpRequest request);
    Task<Response.Base.Response> Reactivate();
    Task<Response.Base.Response> DeactivateAccount();
    Task<Response.Base.Response> DeleteAccount();
}
