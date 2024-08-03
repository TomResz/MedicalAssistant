namespace MedicalAssist.Infrastructure.Email.Factory;
internal interface IEmailSubject
{
	string PasswordChange { get; }	
	string Verification { get; }	
	string CodeRegeneration { get; }	
}
