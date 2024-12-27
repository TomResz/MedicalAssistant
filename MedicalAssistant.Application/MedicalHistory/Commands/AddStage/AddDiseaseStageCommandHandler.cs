using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicalHistory.Commands.AddStage;

internal sealed class AddDiseaseStageCommandHandler
    : ICommandHandler<AddDiseaseStageCommand, Guid>
{
    private readonly IMedicalHistoryRepository _medicalHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVisitRepository _visitRepository;
    public AddDiseaseStageCommandHandler(
        IMedicalHistoryRepository medicalHistoryRepository,
        IUnitOfWork unitOfWork,
        IVisitRepository visitRepository)
    {
        _medicalHistoryRepository = medicalHistoryRepository;
        _unitOfWork = unitOfWork;
        _visitRepository = visitRepository;
    }

    public async Task<Guid> Handle(AddDiseaseStageCommand request, CancellationToken cancellationToken)
    {
        var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(request.MedicalHistoryId, cancellationToken);

        await Validate(request, cancellationToken, medicalHistory);

        DiseaseStage stage = DiseaseStage.Create(
            request.VisitId,
            request.Name,
            request.Date,
            request.Note,
            request.MedicalHistoryId);

        medicalHistory!.AddStage(stage);

        _medicalHistoryRepository.AddStage(stage);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return stage.Id;
    }

    private async Task Validate(
        AddDiseaseStageCommand request, CancellationToken cancellationToken, Domain.Entities.MedicalHistory? medicalHistory)
    {
        if (medicalHistory is null)
        {
            throw new UnknownMedicalHistoryException(request.MedicalHistoryId);
        }

        if (medicalHistory.DiseaseEndDate is not null)
        {
            throw new MedicalHistoryIsCompletedException();
        }
        
        if (request.VisitId is not null)
        {
            var visit = await _visitRepository.GetByIdAsync(request.VisitId, cancellationToken);
            if (visit is null)
            {
                throw new UnknownVisitException();
            }

            if (visit.Date.ToDate() > request.Date.Date)
            {
                throw new MedicalHistoryVisitDateMismatchException();
            }
        }
    }
}