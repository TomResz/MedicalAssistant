using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.User.Commands.VerifyAccount;
public sealed record VerifyAccountCommand(
	string CodeHash) : ICommand;

