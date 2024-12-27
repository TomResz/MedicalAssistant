using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Policies;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Application.MedicationNotifications.Commands.Edit;
internal sealed class EditMedicationNotificationCommandHandler
	: ICommandHandler<EditMedicationNotificationCommand>
{
	private readonly IMedicationRecommendationRepository _medicationRepository;
	private readonly IMedicationRecommendationNotificationRepository _notificationRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMedicationRecommendationNotificationScheduler _scheduler;
	private readonly IMedicationRecommendationPolicy _policy;


	public EditMedicationNotificationCommandHandler(
		IMedicationRecommendationRepository medicationRepository,
		IMedicationRecommendationNotificationRepository notificationRepository,
		IUnitOfWork unitOfWork,
		IMedicationRecommendationNotificationScheduler scheduler,
		IMedicationRecommendationPolicy policy)
	{
		_medicationRepository = medicationRepository;
		_notificationRepository = notificationRepository;
		_unitOfWork = unitOfWork;
		_scheduler = scheduler;
		_policy = policy;
	}

	public async Task Handle(EditMedicationNotificationCommand request, CancellationToken cancellationToken)
	{
		var medication = await _medicationRepository.GetByNotificationIdAsync(request.Id, cancellationToken);

		if (medication is null)
		{
			throw new UnknownMedicationRecommendationNotificationException();
		}

		var notification = medication
			.Notifications
			.First(x => x.Id  == new MedicationRecommendationNotificationId(request.Id));

		Date start = request.Start.Date;
		Date end = request.End.Date.AddDays(1).AddTicks(-1);

		notification.Edit(start,end, request.TriggerTimeUtc);


		if(!_policy.ValidNotificationProperties(medication, notification))
		{
			throw new NotValidMedicationRecommendationNotificationException();
		}


		try
		{
			_scheduler.Schedule(notification.JobId, start, end,request.TriggerTimeUtc,
					notification.MedicationRecommendationId,notification.Id);
			 _notificationRepository.Update(notification);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			_scheduler.Remove(notification.JobId);
			throw;
		}

	}
}
