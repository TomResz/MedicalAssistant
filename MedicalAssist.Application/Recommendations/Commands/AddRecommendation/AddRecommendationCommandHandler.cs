using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.DomainServices;
using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Application.Recommendations.Commands.AddRecommendation;
internal sealed class AddRecommendationCommandHandler : IRequestHandler<AddRecommendationCommand>
{
    private readonly IVisitRepository _visitRepository;
    private readonly IVisitService _visitService;
    private readonly IClock _clock;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public AddRecommendationCommandHandler(IVisitRepository visitRepository, IClock clock, IUserContext userContext, IVisitService visitService, IUnitOfWork unitOfWork)
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
             new(request.MedicineName, request.Quantity, request.TimeOfDay));

        var userId = _userContext.GetUserId;
        var visit = await _visitRepository.GetByIdAsync(request.VisitId, cancellationToken);

        if (visit is null)
        {
            throw new UnknownVisitException();
        }

        _visitService.AddRecommendation(visit, userId, recommendation);

        _visitRepository.Update(visit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
