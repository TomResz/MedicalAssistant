using MediatR;
using MedicalAssistant.Application.User.Commands.SignIn;

namespace MedicalAssistant.Application.User.Commands.FacebookAuthentication;
public sealed record FacebookAuthenticationCommand(
    string Code) : IRequest<SignInResponse>;