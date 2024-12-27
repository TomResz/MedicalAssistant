using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.User.Commands.PasswordChangeByCode;
public sealed record PasswordChangeByCodeCommand(
    string Code,
    string NewPassword) : ICommand;
