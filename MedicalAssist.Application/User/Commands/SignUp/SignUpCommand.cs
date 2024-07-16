using MediatR;

namespace MedicalAssist.Application.User.Commands.SignUp;
public sealed record SignUpCommand(
    string FullName,
    string Email,
    string Password) : IRequest;
