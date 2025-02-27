﻿using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Application.VisitNotifications.Commands.ChangeDate;
internal sealed class ChangeVisitNotificationDateCommandHandler
	: ICommandHandler<ChangeVisitNotificationDateCommand>
{
	private readonly IVisitRepository _visitRepository;
	private readonly IVisitNotificationRepository _notificationRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IVisitNotificationScheduler _scheduler;
	public ChangeVisitNotificationDateCommandHandler(
		IVisitRepository visitRepository,
		IUnitOfWork unitOfWork,
		IVisitNotificationScheduler scheduler,
		IVisitNotificationRepository notificationRepository)
	{
		_visitRepository = visitRepository;
		_unitOfWork = unitOfWork;
		_scheduler = scheduler;
		_notificationRepository = notificationRepository;
	}

	public async Task Handle(ChangeVisitNotificationDateCommand request, CancellationToken cancellationToken)
	{
		var notificationId = new VisitNotificationId(request.Id);
		var date = new Date(request.DateUtc);

		var visit = await _visitRepository.GetByNotificationId(notificationId, cancellationToken);

		if (visit is null)
		{
			throw new UnknownVisitException();
		}

		Validate(request, visit);

		var notification = visit.ChangeNotificationDate(date, notificationId);

		_scheduler.Delete(notification.JobId);
		var jobId = _scheduler.ScheduleJob(visit.Id, notificationId, date.ToDate());

		notification.ChangeJobId(jobId);

		_notificationRepository.Update(notification);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
	}

	private static void Validate(ChangeVisitNotificationDateCommand request, Visit visit)
	{
		if (visit.Date <= new Date(request.CurrentDate))
		{
			throw new InvalidVisitNotificationDateException();
		}

		if (visit.Date <= new Date(request.Date))
		{
			throw new ScheduledDateCannotBeGreatestThanDateException();
		}
	}
}
