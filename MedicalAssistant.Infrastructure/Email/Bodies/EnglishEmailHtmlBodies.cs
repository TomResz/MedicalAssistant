using MedicalAssistant.Infrastructure.Email.Factory;

namespace MedicalAssistant.Infrastructure.Email.Bodies;
internal sealed class EnglishEmailHtmlBodies : IEmailBody
{
    public string GetVerificationCodeHtml(string route, string verificationCode)
    {
        var htmlBody = $@"    
			<p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Press the button below to verify your account.</p>
			<a href='{route}={verificationCode}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Verify Your Account</a>
        ";
        return htmlBody;
    }

    public string GetRegeneratedVerificationCodeHtml(string route, string newVerificationCode)
    {
        var htmlBody = $@"    
			<p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Press the button below to verify your account.</p>
			<a href='{route}={newVerificationCode}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Verify Your Account</a>
        ";
        return htmlBody;
    }

    public string PasswordChange(string route, string code)
    {
        var htmlBody = $@"    
			<p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Press the button below to change your password.</p>
			<a href='{route}={code}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Change Password</a>
        ";
        return htmlBody;
    }
}
