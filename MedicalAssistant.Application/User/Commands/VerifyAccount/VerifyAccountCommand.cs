using MediatR;

namespace MedicalAssistant.Application.User.Commands.VerifyAccount;
public sealed record VerifyAccountCommand(
	string CodeHash) : IRequest;

