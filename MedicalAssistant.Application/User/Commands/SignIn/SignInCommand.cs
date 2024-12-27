using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.User.Commands.SignIn;
public sealed record SignInCommand(
    string Email,
    string Password) : ICommand<SignInResponse>;
