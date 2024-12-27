using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicationNotifications.Commands.Add;
internal sealed class AddMedicationNotificationCommandHandler
	: ICommandHandler<AddMedicationNotificationCommand,Guid>
{
	private readonly IMedicationRecommendationRepository _repository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMedicationRecommendationNotificationScheduler _scheduler;

	public AddMedicationNotificationCommandHandler(
		IMedicationRecommendationRepository repository,
		IUnitOfWork unitOfWork,
		IMedicationRecommendationNotificationScheduler scheduler)
	{
		_repository = repository;
		_unitOfWork = unitOfWork;
		_scheduler = scheduler;
	}

	public async Task<Guid> Handle(AddMedicationNotificationCommand request, CancellationToken cancellationToken)
	{
		MedicationRecommendation? medication = await _repository.GetAsync(request.MedicationRecommendationId, cancellationToken);

		if (medication is null)
		{
			throw new MedicationRecommendationNotFoundException(request.MedicationRecommendationId);
		}


		var notification = MedicationRecommendationNotification.Create(
			medication.Id,
			request.Start,
			request.End,
			request.TriggerTimeUtc);

		try
		{
			medication.AddMedication(notification);

			_scheduler.Schedule(notification.JobId, notification.Start, notification.End, 
				notification.TriggerTimeUtc,medication.Id,notification.Id);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			_scheduler.Remove(notification.JobId);
			throw;
		}
		return notification.Id;
	}
}
