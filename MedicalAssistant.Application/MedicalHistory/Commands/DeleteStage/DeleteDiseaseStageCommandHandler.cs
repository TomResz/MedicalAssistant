using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicalHistory.Commands.DeleteStage;

internal sealed class DeleteDiseaseStageCommandHandler
    : IRequestHandler<DeleteDiseaseStageCommand>
{
    private readonly IMedicalHistoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDiseaseStageCommandHandler(
        IMedicalHistoryRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteDiseaseStageCommand request, CancellationToken cancellationToken)
    {
        var medicalHistory = await _repository.GetByIdAsync(request.MedicalHistoryId, cancellationToken);

        if (medicalHistory is null)
        {
            throw new UnknownMedicalHistoryException(request.MedicalHistoryId);
        }

        medicalHistory.DeleteStage(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}