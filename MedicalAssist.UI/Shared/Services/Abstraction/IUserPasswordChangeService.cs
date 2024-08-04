using MedicalAssist.UI.Models.PasswordChange;

namespace MedicalAssist.UI.Shared.Services.Abstraction;
public interface IUserPasswordChangeService
{
	Task<Response.Base.Response> CheckCode(string code);
	Task<Response.Base.Response> SendMailToChangePassword(ForgotPasswordModel model);
	Task<Response.Base.Response> ChangePasswordByEmail(string code,string newPassword);
}