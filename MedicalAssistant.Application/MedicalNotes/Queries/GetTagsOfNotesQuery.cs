using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalNotes.Queries;

public class GetTagsOfNotesQuery 
        : IQuery<IEnumerable<NoteTagDto>>;