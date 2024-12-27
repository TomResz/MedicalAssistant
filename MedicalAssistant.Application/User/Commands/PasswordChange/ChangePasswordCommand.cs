using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.User.Commands.PasswordChange;
public sealed record ChangePasswordCommand(
    string Password,
    string ConfirmedPassword) : ICommand;
