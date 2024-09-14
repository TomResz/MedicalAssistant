using MediatR;

namespace MedicalAssistant.Application.User.Commands.PasswordChange;
public sealed record ChangePasswordCommand(
    string NewPassword,
    string ConfirmedPassword) : IRequest;
