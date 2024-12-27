using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.User.Commands.PasswordChangeByEmail;
public sealed record PasswordChangeByEmailCommand(
    string Email) : ICommand;
