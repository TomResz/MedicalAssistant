using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicationRecommendations.Commands.DeleteRecommendation;
internal sealed class DeleteRecommendationCommandHandler : IRequestHandler<DeleteRecommendationCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMedicationRecommendationRepository _repository;

	public DeleteRecommendationCommandHandler(
		IUnitOfWork unitOfWork,
		IUserContext userContext,
		IMedicationRecommendationRepository repository)
	{
		_unitOfWork = unitOfWork;
		_userContext = userContext;
		_repository = repository;
	}

	public async Task Handle(DeleteRecommendationCommand request, CancellationToken cancellationToken)
    {
        var recommendation = await _repository.GetAsync(request.Id, cancellationToken);

        if (recommendation is null)
        {
            throw new UnknownRecommendationException(request.Id);
        }

        _repository.Delete(recommendation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
