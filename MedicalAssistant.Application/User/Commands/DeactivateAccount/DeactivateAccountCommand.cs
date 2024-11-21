using MediatR;

namespace MedicalAssistant.Application.User.Commands.DeactivateAccount;

public sealed record DeactivateAccountCommand() : IRequest;