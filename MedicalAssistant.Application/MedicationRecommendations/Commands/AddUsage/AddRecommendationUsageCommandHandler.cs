using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicationRecommendations.Commands.AddUsage;
internal sealed class AddRecommendationUsageCommandHandler : IRequestHandler<AddRecommendationUsageCommand>
{
	private readonly IMedicationRecommendationRepository _repository;
	private readonly IUnitOfWork _unitOfWork;
	public AddRecommendationUsageCommandHandler(
		IMedicationRecommendationRepository repository,
		IUnitOfWork unitOfWork)
	{
		_repository = repository;
		_unitOfWork = unitOfWork;
	}

	public async Task Handle(AddRecommendationUsageCommand request, CancellationToken cancellationToken)
	{
		var recommendation = await _repository.GetAsync(request.RecommendationId, cancellationToken);
		if (recommendation == null)
		{
			throw new UnknownRecommendationException(request.RecommendationId);
		}

		var exists = await _repository.ExistsUsageAsync(
			request.RecommendationId,
			request.Date,
			request.TimeOfDay,
			cancellationToken);

		if (exists)
		{
			return;
		}

		recommendation.AddUsage(request.TimeOfDay, request.Date);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
