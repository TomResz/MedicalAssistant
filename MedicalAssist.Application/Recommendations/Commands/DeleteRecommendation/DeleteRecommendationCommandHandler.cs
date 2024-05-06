using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.Recommendations.Commands.DeleteRecommendation;
internal sealed class DeleteRecommendationCommandHandler : IRequestHandler<DeleteRecommendationCommand>
{
    private readonly IVisitRepository _visitRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteRecommendationCommandHandler(IVisitRepository visitRepository, IUnitOfWork unitOfWork)
    {
        _visitRepository = visitRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteRecommendationCommand request, CancellationToken cancellationToken)
    {
        Visit? visit = await _visitRepository.GetByIdAsync(request.VisitId,cancellationToken);

        if (visit is null)
        {
            throw new UnknownVisitException();
        }

        visit.DeleteRecommendation(request.RecommendationId);
        _visitRepository.Update(visit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
