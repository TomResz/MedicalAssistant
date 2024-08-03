using MedicalAssist.UI.Models.Login;
using MedicalAssist.UI.Shared.Response;
using MedicalAssist.UI.Shared.Services.User;

namespace MedicalAssist.UI.Shared.Services.Abstraction;

public interface IUserAuthService
{
    Task<Response<SignInResponse>> SignIn(LoginModel model);
    Task<Response<SignInResponse>> SignInByFacebook(string code);
    Task<Response<SignInResponse>> SignInByGoogle(string code);

    Task<Response.Response> SignUp(SignUpRequest request);
}
