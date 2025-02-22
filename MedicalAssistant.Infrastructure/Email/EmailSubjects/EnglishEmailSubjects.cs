﻿using MedicalAssistant.Infrastructure.Email.Factory;

namespace MedicalAssistant.Infrastructure.Email.EmailSubjects;
internal class EnglishEmailSubjects : IEmailSubject
{
	public string PasswordChange => "Medical Assistant - Password Change";

	public string Verification => "Medical Assistant - Verification Code";

	public string CodeRegeneration => "Medical Assistant - Verification Code";

	public string VisitNotification => "Medical Assistant - Visit Notification";

	public string MedicationRecommendation =>"Medical Assistant - Medication";
}
