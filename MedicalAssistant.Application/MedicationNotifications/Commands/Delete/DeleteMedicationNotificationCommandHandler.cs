using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicationNotifications.Commands.Delete;
internal sealed class DeleteMedicationNotificationCommandHandler
	: IRequestHandler<DeleteMedicationNotificationCommand>
{
	private readonly IMedicationRecommendationNotificationRepository _repository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMedicationRecommendationNotificationScheduler _notificationScheduler;
	public DeleteMedicationNotificationCommandHandler(
		IMedicationRecommendationNotificationRepository repository,
		IUnitOfWork unitOfWork,
		IMedicationRecommendationNotificationScheduler notificationScheduler)
	{
		_repository = repository;
		_unitOfWork = unitOfWork;
		_notificationScheduler = notificationScheduler;
	}

	public async Task Handle(DeleteMedicationNotificationCommand request, CancellationToken cancellationToken)
	{
		string jobId = $"medication-notification-{request.Id}";
		_notificationScheduler.Remove(jobId);

		var wasDeleted = await _repository.DeleteAsync(request.Id, cancellationToken) > 0;

        if (!wasDeleted)
        {
			throw new UnknownMedicationRecommendationNotificationException();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
