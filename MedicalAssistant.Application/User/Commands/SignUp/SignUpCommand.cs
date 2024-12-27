using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.User.Commands.SignUp;
public sealed record SignUpCommand(
    string FullName,
    string Email,
    string Password) : ICommand;
