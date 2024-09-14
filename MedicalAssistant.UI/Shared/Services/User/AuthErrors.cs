namespace MedicalAssistant.UI.Shared.Services.User;

public sealed class AuthErrors 
{
	public const string UnverifiedUser = nameof(UnverifiedUser);
	public const string InvalidLoginProvider = nameof(InvalidLoginProvider);
	public const string InvalidSignInCredentials = nameof(InvalidSignInCredentials);
	public const string InvalidExternalProvider = nameof(InvalidExternalProvider);
	public const string EmailInUse = nameof(EmailInUse);
}
