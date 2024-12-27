using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.User.Commands.RegenerateVerificationCode;
public sealed record RegenerateVerificationCodeCommand(
	string Email) : ICommand;
