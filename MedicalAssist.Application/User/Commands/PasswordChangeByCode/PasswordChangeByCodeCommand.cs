using MediatR;

namespace MedicalAssist.Application.User.Commands.PasswordChangeByCode;
public sealed record PasswordChangeByCodeCommand(
    string Code,
    string NewPassword) : IRequest;
