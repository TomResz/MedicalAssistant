using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Dto;
using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Application.VisitNotifications.Commands.AddNotifications;

internal sealed class AddVisitNotificationCommandHandler
	: IRequestHandler<AddVisitNotificationCommand, VisitNotificationDto>
{
	private readonly IVisitRepository _visitRepository;
	private readonly IVisitNotificationRepository _notificationRepository;
	private readonly IUserContext _userContext;
	private readonly IVisitNotificationScheduler _notificationScheduler;
	private readonly IUnitOfWork _unitOfWork;

	public AddVisitNotificationCommandHandler(
		IVisitRepository visitRepository,
		IUserContext userContext,
		IVisitNotificationScheduler notificationScheduler,
		IUnitOfWork unitOfWork,
		IVisitNotificationRepository notificationRepository)
	{
		_visitRepository = visitRepository;
		_userContext = userContext;
		_notificationScheduler = notificationScheduler;
		_unitOfWork = unitOfWork;
		_notificationRepository = notificationRepository;
	}

	public async Task<VisitNotificationDto> Handle(AddVisitNotificationCommand request, CancellationToken cancellationToken)
	{
		var date = new Date(request.ScheduledDateUtc);
		var userId = _userContext.GetUserId;
		var visitId = new VisitId(request.VisitId);
		var visit = await _visitRepository.GetByIdWithNotificationsAsync(visitId, cancellationToken);

		if (visit is null)
		{
			throw new UnknownVisitException();
		}

		VisitNotificationId notificationId = Guid.NewGuid();
		string jobId = _notificationScheduler.ScheduleJob(visitId, notificationId, date);
		
		try
		{
			var visitNotification = VisitNotification.Create(
				notificationId,
				  userId,
				jobId,
				date,
				   visitId);

			visit.AddNotification(visitNotification);
			
			_notificationRepository.Add(visitNotification);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return new VisitNotificationDto()
			{
				Id = visitNotification.Id,
				ScheduledDateUtc = date,
				SimpleId = visitNotification.SimpleId,
				VisitId = visitNotification.VisitId
			};
		}
		catch (Exception)
		{
			_notificationScheduler.Delete(jobId);
			throw;
		}

	}
}