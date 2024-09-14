using MediatR;

namespace MedicalAssistant.Application.User.Commands.RegenerateVerificationCode;
public sealed record RegenerateVerificationCodeCommand(
	string Email) : IRequest;
