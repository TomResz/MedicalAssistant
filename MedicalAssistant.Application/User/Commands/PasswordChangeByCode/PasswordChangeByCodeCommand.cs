using MediatR;

namespace MedicalAssistant.Application.User.Commands.PasswordChangeByCode;
public sealed record PasswordChangeByCodeCommand(
    string Code,
    string NewPassword) : IRequest;
