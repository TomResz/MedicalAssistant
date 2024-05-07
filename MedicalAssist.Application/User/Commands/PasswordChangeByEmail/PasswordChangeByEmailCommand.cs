using MediatR;

namespace MedicalAssist.Application.User.Commands.PasswordChangeByEmail;
public sealed record PasswordChangeByEmailCommand(
    string Email) : IRequest;
