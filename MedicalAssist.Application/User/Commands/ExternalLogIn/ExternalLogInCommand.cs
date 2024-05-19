using MediatR;
using MedicalAssist.Application.User.Commands.SignIn;
using System.Security.Claims;

namespace MedicalAssist.Application.User.Commands.ExternalLogIn;
public sealed record ExternalLogInCommand(
    ClaimsPrincipal Claims,string? Schema) : IRequest<SignInResponse>;
