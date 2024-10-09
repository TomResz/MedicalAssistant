using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Application.VisitNotifications.Commands.DeleteNotification;
internal sealed class DeleteVisitNotificationCommandHandler
	: IRequestHandler<DeleteVisitNotificationCommand>
{
	private readonly IVisitNotificationScheduler _notificationScheduler;
	private readonly IVisitRepository _visitRepository;
	private readonly IVisitNotificationRepository _visitNotificationRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IClock _clock;
	public DeleteVisitNotificationCommandHandler(
		IVisitNotificationScheduler notificationScheduler,
		IVisitRepository visitRepository,
		IUnitOfWork unitOfWork,
		IVisitNotificationRepository visitNotificationRepository,
		IClock clock)
	{
		_notificationScheduler = notificationScheduler;
		_visitRepository = visitRepository;
		_unitOfWork = unitOfWork;
		_visitNotificationRepository = visitNotificationRepository;
		_clock = clock;
	}

	public async Task Handle(DeleteVisitNotificationCommand request, CancellationToken cancellationToken)
	{
		Visit? visit = await _visitRepository.GetByNotificationId(request.VisitNotificationId, cancellationToken);

        if (visit is null)
        {
			throw new UnknownVisitException();
        }
		
		var notification = visit
			.Notifications
			.Where(x=>x.Id == new VisitNotificationId(request.VisitNotificationId))
			.FirstOrDefault();
		
		if(notification is null)
		{
			throw new UnknownVisitNotificationException(request.VisitNotificationId);
		}
		else if(notification.ScheduledDateUtc < new Date(_clock.GetCurrentUtc()))
		{
			throw new NotificationHasAlreadyBeenSentException(notification.Id);
		}

		string jobId = notification.SimpleId;


		_visitNotificationRepository.Delete(notification);
		_notificationScheduler.Delete(jobId);
		visit.DeleteNotification(notification);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
