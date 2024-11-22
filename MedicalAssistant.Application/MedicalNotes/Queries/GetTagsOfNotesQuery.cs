using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalNotes.Queries;

public class GetTagsOfNotesQuery 
        : IRequest<IEnumerable<NoteTagDto>>;