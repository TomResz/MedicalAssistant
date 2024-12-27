using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicalHistory.Commands.Delete;

internal sealed class DeleteMedicalHistoryCommandHandler : ICommandHandler<DeleteMedicalHistoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMedicalHistoryRepository _repository;
    
    public DeleteMedicalHistoryCommandHandler(
        IUnitOfWork unitOfWork,
        IMedicalHistoryRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task Handle(DeleteMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var isDeleted = await _repository.DeleteAsync(request.Id, cancellationToken);

        if (!isDeleted)
        {
            throw new UnknownMedicalHistoryException(request.Id);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}