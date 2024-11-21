using MediatR;

namespace MedicalAssistant.Application.User.Commands.DeleteAccount;

public sealed record DeleteAccountCommand() 
    : IRequest;