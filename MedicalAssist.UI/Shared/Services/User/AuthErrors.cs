using MedicalAssist.UI.Shared.Response;

namespace MedicalAssist.UI.Shared.Services.User;

public sealed class AuthErrors : Error
{
	public static readonly  AuthErrors UnverifiedUser = new();
	public static readonly AuthErrors InvalidLoginProvider = new();
	public static readonly AuthErrors InvalidSignInCredentials = new();
	public static readonly AuthErrors InvalidExternalProvider = new();
	public static readonly AuthErrors EmailInUse = new();
}
