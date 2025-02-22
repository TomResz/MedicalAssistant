﻿using MedicalAssistant.UI.Models.PasswordChange;
using MedicalAssistant.UI.Shared.Requests;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;

namespace MedicalAssistant.UI.Shared.Services.User;

public sealed class UserPasswordChangeService : IUserPasswordChangeService
{
	private readonly HttpClient _httpClient;

	public UserPasswordChangeService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response.Base.Response> SendMailToChangePassword(ForgotPasswordModel model)
	{
		var response = await _httpClient.PostAsJsonAsync("user/password-change", model);
		return await response.DeserializeResponse();
	}

	public async Task<Response.Base.Response> CheckCode(string code)
	{
		var response = await _httpClient.PostAsync($"user/check-password-code/{code}",null);	
		return await response.DeserializeResponse();
	}

	public async Task<Response.Base.Response> ChangePasswordByEmail(string code, string newPassword)
	{
		var response = await _httpClient.PutAsJsonAsync("user/password-change-by-code",
			new ChangePasswordByEmailRequest { Code = code, NewPassword = newPassword });
		return await response.DeserializeResponse();
	}

	public async Task<Response.Base.Response> ChangePassword(ChangePasswordModel model)
	{
		var resonse = await _httpClient.PutAsJsonAsync("user/password-change-auth", model);
		return await resonse.DeserializeResponse();
	}
}
