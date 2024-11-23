using System.Net;
using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Events;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.User.Commands.DeleteAccount;

internal sealed class DeleteAccountCommandHandler
    : IRequestHandler<DeleteAccountCommand>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;
    public DeleteAccountCommandHandler(
        IUserContext userContext,
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork,
        IEventPublisher eventPublisher)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(_userContext.GetUserId,cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        _userRepository.Delete(user);
        await _eventPublisher.EnqueueEventAsync(new AccountDeletedEvent(user.Id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}