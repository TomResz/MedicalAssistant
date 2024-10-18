using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.DomainServices;
using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicationRecommendations.Commands.AddRecommendation;
internal sealed class AddMedicationRecommendationCommandHandler 
	: IRequestHandler<AddMedicationRecommendationCommand, AddMedicationRecommendationResponse>
{
    private readonly IVisitRepository _visitRepository;
    private readonly IVisitService _visitService;
    private readonly IClock _clock;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

	public AddMedicationRecommendationCommandHandler(
		IVisitRepository visitRepository,
		IClock clock,
		IUserContext userContext,
		IVisitService visitService,
		IUnitOfWork unitOfWork,
		IUserRepository userRepository)
	{
		_visitRepository = visitRepository;
		_clock = clock;
		_userContext = userContext;
		_visitService = visitService;
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
	}

	public async Task<AddMedicationRecommendationResponse> Handle(AddMedicationRecommendationCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;

		MedicationRecommendation recommendation = MedicationRecommendation.Create(
	    request.VisitId,
		    userId,
	    request.ExtraNote,
	    _clock.GetCurrentUtc(),
	    new(request.MedicineName, request.Quantity, request.TimeOfDay),
	    request.StartDate,
	    request.EndDate);

		VisitDto? _visitDto = null;

		if (request.VisitId is not null)
        {
            var visit = await _visitRepository.GetByIdWithRecommendationsAsync(request.VisitId, cancellationToken);

            if (visit is null)
            {
                throw new UnknownVisitException();
            }

            _visitService.AddRecommendation(visit, userId, recommendation);

            _visitRepository.Update(visit);
			_visitDto = visit.ToDto();
        }
        else
        {
			var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
			
			if(user is null)
			{
				throw new UserNotFoundException();
			}

			user.AddMedicationRecommendation(recommendation);
			_userRepository.Update(user);

        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

		return new AddMedicationRecommendationResponse
		{
			Id = recommendation.Id,
			Visit = _visitDto,
		};
    }
}
