using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Application.MedicalNotes.Commands.Add;

internal sealed class AddMedicalNoteCommandHandler : IRequestHandler<AddMedicalNoteCommand,Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMedicalNoteRepository _medicalNoteRepository;
    
    public AddMedicalNoteCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IMedicalNoteRepository medicalNoteRepository)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _medicalNoteRepository = medicalNoteRepository;
    }

    public async Task<Guid> Handle(AddMedicalNoteCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;
        
        var medicalNote = MedicalNote.Create(
            userId,
            request.Note,
            request.CreatedAt,
            request.Tags);
        
        _medicalNoteRepository.Add(medicalNote);
        await _unitOfWork.SaveChangesAsync(cancellationToken);  
        return medicalNote.Id;
    }
}