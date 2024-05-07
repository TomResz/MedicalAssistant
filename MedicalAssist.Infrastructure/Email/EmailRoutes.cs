namespace MedicalAssist.Infrastructure.Email;
internal sealed class EmailRoutes
{
	public string VerificationCodeRoute { get; set; }
	public string RegeneratedVerificationCodeRoute { get; set; }
    public string PasswordChange { get; set; }
}
