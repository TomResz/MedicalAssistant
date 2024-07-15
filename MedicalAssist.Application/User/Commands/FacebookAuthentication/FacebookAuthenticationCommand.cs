using MediatR;
using MedicalAssist.Application.User.Commands.SignIn;

namespace MedicalAssist.Application.User.Commands.FacebookAuthentication;
public sealed record FacebookAuthenticationCommand(
    string Code) : IRequest<SignInResponse>;