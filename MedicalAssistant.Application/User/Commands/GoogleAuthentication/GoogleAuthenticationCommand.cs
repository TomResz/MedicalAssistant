using MediatR;
using MedicalAssistant.Application.User.Commands.SignIn;

namespace MedicalAssistant.Application.User.Commands.GoogleAuthentication;
public sealed record GoogleAuthenticationCommand(
    string Code) : IRequest<SignInResponse>;
