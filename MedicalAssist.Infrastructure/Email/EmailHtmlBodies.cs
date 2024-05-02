using System.Net.Mail;

namespace MedicalAssist.Infrastructure.Email;
internal sealed class EmailHtmlBodies
{
	public static string GetVerificationCodeHtml(string route,string verificationCode)
	{
		var htmlBody = $@"    
			<p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>This is your verification code: <b>{verificationCode}</b>.</p>
			<p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Press the button below to verify your account.</p>
			<a href='{route}={verificationCode}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Verify Your Account</a>
        ";
		return htmlBody;
	}

	public static string GetRegeneratedVerificationCodeHtml(string route, string newVerificationCode)
	{
		var htmlBody = $@"    
			<p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>This is your verification code: <b>{newVerificationCode}</b>.</p>
			<p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Press the button below to verify your account.</p>
			<a href='{route}={newVerificationCode}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Verify Your Account</a>
        ";
		return htmlBody;
	}
}
