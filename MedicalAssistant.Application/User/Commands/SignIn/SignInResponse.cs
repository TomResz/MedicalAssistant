namespace MedicalAssistant.Application.User.Commands.SignIn;
public sealed record SignInResponse(
    string AccessToken,
    string RefreshToken);