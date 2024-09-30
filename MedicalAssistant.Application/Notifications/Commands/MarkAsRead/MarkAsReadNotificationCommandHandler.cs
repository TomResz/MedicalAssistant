using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.Notifications.Commands.MarkAsRead;
internal sealed class MarkAsReadNotificationCommandHandler : IRequestHandler<MarkAsReadNotificationCommand>
{
	private readonly INotificationHistoryRepository _historyRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IClock _clock;

	public MarkAsReadNotificationCommandHandler(
		INotificationHistoryRepository historyRepository,
		IUnitOfWork unitOfWork,
		IClock clock)
	{
		_historyRepository = historyRepository;
		_unitOfWork = unitOfWork;
		_clock = clock;
	}

	public async Task Handle(MarkAsReadNotificationCommand request, CancellationToken cancellationToken)
	{
		await _historyRepository.MarkAsReadRange(request.IDs,_clock.GetCurrentUtc());
		await _unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
