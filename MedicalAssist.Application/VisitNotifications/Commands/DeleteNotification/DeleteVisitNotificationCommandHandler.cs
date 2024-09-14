using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Application.VisitNotifications.Commands.DeleteNotification;
internal sealed class DeleteVisitNotificationCommandHandler
	: IRequestHandler<DeleteVisitNotificationCommand>
{
	private readonly IVisitNotificationScheduler _notificationScheduler;
	private readonly IVisitRepository _visitRepository;
	private readonly IVisitNotificationRepository _visitNotificationRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteVisitNotificationCommandHandler(
		IVisitNotificationScheduler notificationScheduler,
		IVisitRepository visitRepository,
		IUnitOfWork unitOfWork,
		IVisitNotificationRepository visitNotificationRepository)
	{
		_notificationScheduler = notificationScheduler;
		_visitRepository = visitRepository;
		_unitOfWork = unitOfWork;
		_visitNotificationRepository = visitNotificationRepository;
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

		string jobId = notification.SimpleId;


		_visitNotificationRepository.Delete(notification);
		_notificationScheduler.Delete(jobId);
		visit.DeleteNotification(notification);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
