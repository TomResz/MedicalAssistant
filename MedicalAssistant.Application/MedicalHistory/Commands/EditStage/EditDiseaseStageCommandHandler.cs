using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects.IDs;
using UnknownDiseaseStageException = MedicalAssistant.Application.Exceptions.UnknownDiseaseStageException;

namespace MedicalAssistant.Application.MedicalHistory.Commands.EditStage;

internal sealed class EditDiseaseStageCommandHandler
    : IRequestHandler<EditDiseaseStageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMedicalHistoryRepository _medicalHistoryRepository;
    private readonly IVisitRepository _visitRepository;

    public EditDiseaseStageCommandHandler(
        IUnitOfWork unitOfWork,
        IMedicalHistoryRepository medicalHistoryRepository,
        IVisitRepository visitRepository)
    {
        _unitOfWork = unitOfWork;
        _medicalHistoryRepository = medicalHistoryRepository;
        _visitRepository = visitRepository;
    }

    public async Task Handle(EditDiseaseStageCommand request, CancellationToken cancellationToken)
    {
        var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(request.MedicalHistoryId, cancellationToken);

        if (medicalHistory is null)
        {
            throw new UnknownMedicalHistoryException(request.MedicalHistoryId);
        }

        var stage = medicalHistory
            .DiseaseStages
            .FirstOrDefault(x => x.Id == new DiseaseStageId(request.Id));

        if (stage is null)
        {
            throw new UnknownDiseaseStageException(request.Id);
        }

        await Validate(medicalHistory,request, stage, cancellationToken);

        medicalHistory.EditStage(stage,
            request.Name,
            request.Note,
            request.VisitId,
            request.Date);
        
        _medicalHistoryRepository.UpdateStage(stage);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task Validate(
        Domain.Entities.MedicalHistory medicalHistory,
        EditDiseaseStageCommand request, 
        DiseaseStage stage,
        CancellationToken cancellationToken)
    {
        var requestDate = request.Date.Date;
        
        if (stage.VisitId is not null &&
            request.VisitId is not null &&
            stage.VisitId.Value != request.VisitId &&
            stage.Date.ToDate() != requestDate)
        {
            var visit = await _visitRepository.GetByIdAsync(request.VisitId, cancellationToken);

            if (visit is null)
            {
                throw new UnknownVisitException();
            }

            if (visit.Date.ToDate() != requestDate)
            {
                throw new InvalidStageDateException();
            }
        }
    }
}