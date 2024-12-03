using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Events;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.Extensions.Logging;

namespace MedicalAssistant.Application.User.Events;

internal sealed class AccountDeletedOrDeactivatedEventsHandler
    : INotificationHandler<AccountDeactivatedEvent>,
        INotificationHandler<AccountDeletedEvent>
{
    private readonly IRefreshTokenRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AccountDeletedOrDeactivatedEventsHandler> _logger;
    
    public AccountDeletedOrDeactivatedEventsHandler(
        IRefreshTokenRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<AccountDeletedOrDeactivatedEventsHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(AccountDeactivatedEvent notification, CancellationToken cancellationToken)
        => await DeleteAllTokens(notification.UserId, cancellationToken);

    public async Task Handle(AccountDeletedEvent notification, CancellationToken cancellationToken)
        => await DeleteAllTokens(notification.UserId, cancellationToken);

    private async Task DeleteAllTokens(UserId userId,CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(userId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("All refresh tokens for UserId {UserId} have been deleted successfully.", userId);

    }
}