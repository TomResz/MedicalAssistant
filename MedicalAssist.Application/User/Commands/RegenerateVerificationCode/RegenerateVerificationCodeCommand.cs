using MediatR;

namespace MedicalAssist.Application.User.Commands.RegenerateVerificationCode;
public sealed record RegenerateVerificationCodeCommand(
	string Email) : IRequest;
