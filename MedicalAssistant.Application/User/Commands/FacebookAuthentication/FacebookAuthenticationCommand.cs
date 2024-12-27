using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.User.Commands.SignIn;

namespace MedicalAssistant.Application.User.Commands.FacebookAuthentication;
public sealed record FacebookAuthenticationCommand(
    string Code) : ICommand<SignInResponse>;