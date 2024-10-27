using MedicalAssistant.Application.Dto;
using MedicalAssistant.Infrastructure.Email.Factory;
using Microsoft.AspNetCore.Routing;

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

	public string VisitNotification(VisitDto visitDto, string route)
	{
		var htmlBody = $@"
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;''><b>Visit Notification</b></p>
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Doctor: <b>{visitDto.DoctorName}</b>.</p>
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Visit Type: <b>{visitDto.VisitType}</b></p>
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Date: <b>{visitDto.Date.ToString("HH:mm dd-MM-yyyy")}</b></p>
        <a href='{route}={visitDto.VisitId}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Show details</a>        
    ";
		return htmlBody;
	}

	public string MedicationRecommendation(string route, MedicationRecommendationDto dto)
	{
		var htmlBody = $@"
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;''><b>Medication Notification</b></p>
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Reminder to take the medicine - <b>{dto.Name}</b>.</p>
        <a href='{route}/{dto.Id}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Show details</a>        
    ";
		return htmlBody;
	}
}
