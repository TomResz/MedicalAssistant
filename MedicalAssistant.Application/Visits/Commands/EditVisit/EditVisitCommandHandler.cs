using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Application.Visits.Commands.EditVisit;
internal sealed class EditVisitCommandHandler : IRequestHandler<EditVisitCommand,VisitDto>
{
	private readonly IVisitRepository _visitRepository;
	private readonly IUnitOfWork _unitOfWork;

	public EditVisitCommandHandler(
		IVisitRepository visitRepository,
		IUnitOfWork unitOfWork)
	{
		_visitRepository = visitRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<VisitDto> Handle(EditVisitCommand request, CancellationToken cancellationToken)
	{
		Address address = new(request.Street, request.City, request.PostalCode);
		DoctorName doctorName = request.DoctorName;
		VisitDescription description = request.VisitDescription;
		VisitType visitType = request.VisitType;
		Date date = request.Date;
		Date endDate = request.PredictedEndDate;


		var visit = await _visitRepository.GetByIdAsync(request.Id, cancellationToken);

		if (visit is null)
		{
			throw new UnknownVisitException(request.Id);
		}

		bool hasConflictingVisits = await _visitRepository.HasConflictingVisits(
			   request.Id, 
			   visit.UserId,
			   date, 
				 endDate,
				   cancellationToken);

		if(hasConflictingVisits)
		{
			throw new VisitAlreadyExistsForGivenPeriodOfTimeException(date, endDate);
		}

		visit.Update(
			address,
			   date,
			   doctorName,
			   description,
			   visitType,
			   endDate);

		_visitRepository.Update(visit);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return visit.ToDto();
	}
}
