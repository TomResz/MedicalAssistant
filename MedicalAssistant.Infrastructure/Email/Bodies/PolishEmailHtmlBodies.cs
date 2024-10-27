using MedicalAssistant.Application.Dto;
using MedicalAssistant.Infrastructure.Email.Factory;
using Microsoft.AspNetCore.Routing;

namespace MedicalAssistant.Infrastructure.Email.Bodies;
internal sealed class PolishEmailHtmlBodies : IEmailBody
{
	public string GetVerificationCodeHtml(string route, string verificationCode)
	{
		var htmlBody = $@"    
			<p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Naciśnij przycisk aby zweryfikować konto.</p>
			<a href='{route}={verificationCode}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Zweryfikuj konto</a>
        ";
		return htmlBody;
	}

	public string GetRegeneratedVerificationCodeHtml(string route, string newVerificationCode)
	{
		var htmlBody = $@"    
			<p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Naciśnij przycisk aby zweryfikować konto.</p>
			<a href='{route}={newVerificationCode}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Zweryfikuj konto</a>
        ";
		return htmlBody;
	}

	public string PasswordChange(string route, string code)
	{
		var htmlBody = $@"    
			<p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Naciśnij przycisk aby zmienić hasło.</p>
			<a href='{route}={code}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Zmień hasło</a>
        ";
		return htmlBody;
	}

	public string VisitNotification(VisitDto visitDto,string route)
	{
		var htmlBody = $@"
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;''><b>Przypomnienia O Wizycie</b></p>
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Lekarz: <b>{visitDto.DoctorName}</b>.</p>
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Typ wizyty: <b>{visitDto.VisitType}</b></p>
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Data: <b>{visitDto.Date.ToString("HH:mm dd-MM-yyyy")}</b></p>
        <a href='{route}={visitDto.VisitId}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Zobacz szczegóły</a>        
    ";
		return htmlBody;
	}

	public string MedicationRecommendation(string route, MedicationRecommendationDto dto)
	{
		var htmlBody = $@"
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;''><b>Przypomnienie o Lekarstwach</b></p>
        <p style='font-family: Arial, sans-serif; color: #333; font-size: 20px;'>Przypomnienie o zażyciu leku:- <b>{dto.Name}</b>.</p>
        <a href='{route}/{dto.Id}' style='text-decoration: none; background-color: #007bff; color: #fff; padding: 15px 30px; font-family: Arial, sans-serif; font-size: 20px; border-radius: 5px; cursor: pointer; display: inline-block;'>Zobacz Szczegóły</a>        
    ";
		return htmlBody;
	}
}
