using MediatR;

namespace MedicalAssist.Application.User.Commands.SignIn;
public sealed record SignInCommand(
    string Email,
    string Password) : IRequest<SignInResponse>;
