using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicalNotes.Commands.Delete;

internal sealed class DeleteMedicalNoteCommandHandler : ICommandHandler<DeleteMedicalNoteCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMedicalNoteRepository _repository;

    public DeleteMedicalNoteCommandHandler(
        IUnitOfWork unitOfWork,
        IMedicalNoteRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task Handle(DeleteMedicalNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _repository.GetById(request.Id, cancellationToken);
        
        if (note is null)
        {
            throw new UnknownMedicalHistoryException(request.Id);
        }
        
        _repository.Delete(note);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}