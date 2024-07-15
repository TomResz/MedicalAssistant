namespace MedicalAssist.Application.User.Commands.SignIn;
public sealed record SignInResponse(
    string Role,
    string FullName,
    string Token,
    string RefreshToken);