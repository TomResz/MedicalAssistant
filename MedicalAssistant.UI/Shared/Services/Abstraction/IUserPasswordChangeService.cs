using MedicalAssistant.UI.Models.PasswordChange;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;
public interface IUserPasswordChangeService
{
	Task<Response.Base.Response> CheckCode(string code);
	Task<Response.Base.Response> SendMailToChangePassword(ForgotPasswordModel model);
	Task<Response.Base.Response> ChangePasswordByEmail(string code,string newPassword);
	Task<Response.Base.Response> ChangePassword(ChangePasswordModel model);
}