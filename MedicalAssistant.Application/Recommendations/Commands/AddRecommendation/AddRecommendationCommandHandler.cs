using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.DomainServices;
using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.Recommendations.Commands.AddRecommendation;
internal sealed class AddRecommendationCommandHandler : IRequestHandler<AddRecommendationCommand>
{
    private readonly IVisitRepository _visitRepository;
    private readonly IVisitService _visitService;
    private readonly IClock _clock;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public AddRecommendationCommandHandler(
		IVisitRepository visitRepository,
		IClock clock,
		IUserContext userContext,
		IVisitService visitService,
		IUnitOfWork unitOfWork)
    {
        _visitRepository = visitRepository;
        _clock = clock;
        _userContext = userContext;
        _visitService = visitService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddRecommendationCommand request, CancellationToken cancellationToken)
    {
        Recommendation recommendation = Recommendation.Create(
            request.VisitId,
             request.ExtraNote,
             _clock.GetCurrentUtc(),
             new(request.MedicineName, request.Quantity, request.TimeOfDay),
             request.StartDate,
             request.EndDate);

        var userId = _userContext.GetUserId;
        var visit = await _visitRepository.GetByIdWithRecommendationsAsync(request.VisitId, cancellationToken);

        if (visit is null)
        {
            throw new UnknownVisitException();
        }

        _visitService.AddRecommendation(visit, userId, recommendation);

        _visitRepository.Update(visit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
