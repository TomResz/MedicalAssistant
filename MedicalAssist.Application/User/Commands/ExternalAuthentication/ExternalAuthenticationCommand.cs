using MediatR;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.User.Commands.SignIn;

namespace MedicalAssist.Application.User.Commands.ExternalAuthentication;
internal sealed record ExternalAuthenticationCommand(
	ExternalApiResponse? response,
	string Provider) : IRequest<SignInResponse>;
