using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicalNotes.Commands.Edit;

internal sealed class EditMedicalNoteCommandHandler : IRequestHandler<EditMedicalNoteCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMedicalNoteRepository _repository;
    
    public EditMedicalNoteCommandHandler(
        IUnitOfWork unitOfWork,
        IMedicalNoteRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task Handle(EditMedicalNoteCommand request, CancellationToken cancellationToken)
    {
        MedicalNote? note = await _repository.GetById(request.Id, cancellationToken);

        if (note is null)
        {
            throw new UnknownMedicalNoteException(request.Id);
        }
        
        note.Update(request.Note, request.Tags);
        _repository.Update(note);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}