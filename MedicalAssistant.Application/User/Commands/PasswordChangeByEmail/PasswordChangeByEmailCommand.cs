using MediatR;

namespace MedicalAssistant.Application.User.Commands.PasswordChangeByEmail;
public sealed record PasswordChangeByEmailCommand(
    string Email) : IRequest;
