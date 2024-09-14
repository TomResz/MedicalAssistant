using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.DomainServices;
using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.Recommendations.Commands.DeleteRecommendation;
internal sealed class DeleteRecommendationCommandHandler : IRequestHandler<DeleteRecommendationCommand>
{
    private readonly IVisitRepository _visitRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVisitService _visitService;
    private readonly IUserContext _userContext;
    
    public DeleteRecommendationCommandHandler(
		IVisitRepository visitRepository,
		IUnitOfWork unitOfWork,
		IVisitService visitService,
		IUserContext userContext)
    {
        _visitRepository = visitRepository;
        _unitOfWork = unitOfWork;
        _visitService = visitService;
        _userContext = userContext;
    }

    public async Task Handle(DeleteRecommendationCommand request, CancellationToken cancellationToken)
    {
        Visit? visit = await _visitRepository.GetByIdWithRecommendationsAsync(request.VisitId,cancellationToken);

        if (visit is null)
        {
            throw new UnknownVisitException(request.RecommendationId);
        }

        _visitService.RemoveRecommendation(visit, _userContext.GetUserId, request.RecommendationId);

        _visitRepository.Update(visit);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
