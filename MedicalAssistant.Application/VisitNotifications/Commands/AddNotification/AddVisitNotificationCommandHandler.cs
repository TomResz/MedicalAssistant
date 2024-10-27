using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Application.VisitNotifications.Commands.AddNotifications;

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

		Validate(request, visit);

		VisitNotificationId notificationId = Guid.NewGuid();
		string jobId = _notificationScheduler.ScheduleJob(visitId, notificationId, date.ToDate());

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

			return visitNotification.ToDto();
		}
		catch (Exception)
		{
			_notificationScheduler.Delete(jobId);
			throw;
		}

	}

	private static void Validate(AddVisitNotificationCommand request, Visit visit)
	{
		if (visit.Date <= new Date(request.CurrentDate))
		{
			throw new InvalidVisitNotificationDateException();
		}

		if (visit.Date <= new Date(request.ScheduledDate))
		{
			throw new ScheduledDateCannotBeGreatestThanDateException();
		}
	}
}