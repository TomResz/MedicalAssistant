using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicalHistory.Commands.AddStage;

internal sealed class AddDiseaseStageCommandHandler
    : IRequestHandler<AddDiseaseStageCommand, Guid>
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

        if (medicalHistory is null)
        {
            throw new UnknownMedicalHistoryException(request.MedicalHistoryId);
        }

        if (request.VisitId is not null)
        {
            var visit = await _visitRepository.GetByIdAsync(request.VisitId, cancellationToken);
            if (visit is null)
            {
                throw new UnknownVisitException();
            }
        }

        DiseaseStage stage = DiseaseStage.Create(
            request.VisitId,
            request.Name,
            request.Date,
            request.Note,
            request.MedicalHistoryId);

        medicalHistory.AddStage(stage);

        _medicalHistoryRepository.Update(medicalHistory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return stage.Id;
    }
}