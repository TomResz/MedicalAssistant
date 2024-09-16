﻿namespace MedicalAssistant.Infrastructure.Email;
internal sealed class EmailClientSettings
{
	public string Email { get; set; }
	public string Password { get; set; }
	public int Port { get; set; }
	public string Host { get; set; }
}