using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Application.MedicationRecommendations.Commands.UpdateRecommendation;
internal sealed class UpdateMedicationRecommendationCommandHandler
	: IRequestHandler<UpdateMedicationRecommendationCommand, VisitDto?>
{
	private readonly IMedicationRecommendationRepository _medicationRepository;
	private readonly IVisitRepository _visitRepository;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateMedicationRecommendationCommandHandler(
		IMedicationRecommendationRepository medicationRepository,
		IVisitRepository visitRepository,
		IUnitOfWork unitOfWork)
	{
		_medicationRepository = medicationRepository;
		_visitRepository = visitRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<VisitDto?> Handle(UpdateMedicationRecommendationCommand request, CancellationToken cancellationToken)
	{
		var recommendation = await _medicationRepository.GetAsync(request.Id, cancellationToken);

		if (recommendation is null)
		{
			throw new UnknownRecommendationException(request.Id);
		}

		var medicine = new Medicine(request.MedicineName, request.Quantity, request.TimeOfDay);
		var start = new Date(request.StartDate);
		var end = new Date(request.EndDate);

		VisitDto? visitDto = null;

		if (request.VisitId is not null)
		{
			var visit = await _visitRepository.GetByIdAsync(request.VisitId, cancellationToken);
			if(visit is null)
			{
				throw new UnknownVisitException();
			}

			visitDto = visit.ToDto();
		}

		recommendation.Update(medicine,start, end, visitDto?.VisitId,request.ExtraNote);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return visitDto;
	}
}
