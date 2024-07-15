using MediatR;
using MedicalAssist.Application.User.Commands.SignIn;

namespace MedicalAssist.Application.User.Commands.GoogleAuthentication;
public sealed record GoogleAuthenticationCommand(
    string Code) : IRequest<SignInResponse>;
