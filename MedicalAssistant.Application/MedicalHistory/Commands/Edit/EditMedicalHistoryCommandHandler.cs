using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicalHistory.Commands.Edit;

internal sealed class EditMedicalHistoryCommandHandler : ICommandHandler<EditMedicalHistoryCommand>
{
    private readonly IMedicalHistoryRepository _medicalHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVisitRepository _visitRepository;
    
    public EditMedicalHistoryCommandHandler(
        IMedicalHistoryRepository medicalHistoryRepository,
        IUnitOfWork unitOfWork,
        IVisitRepository visitRepository)
    {
        _medicalHistoryRepository = medicalHistoryRepository;
        _unitOfWork = unitOfWork;
        _visitRepository = visitRepository;
    }

    public async Task Handle(EditMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (medicalHistory is null)
        {
            throw new UnknownMedicalHistoryException(request.Id);
        }
        
        if (request.VisitId is not null)
        {
            var visit = await _visitRepository.GetByIdWithMedicalHistoryAsync(request.VisitId, cancellationToken);

            if (visit is null)
            {
                throw new UnknownVisitException();
            }

            if (request.StartDate < visit.Date)
            {
                throw new InvalidMedicalHistoryStartDateException();
            }
        }
        medicalHistory.Edit(
            request.Name,
            request.StartDate,
            request.EndDate,
            request.SymptomDescription,
            request.Notes,
            request.VisitId);
        
        _medicalHistoryRepository.Update(medicalHistory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}