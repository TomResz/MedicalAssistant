namespace MedicalAssistant.Domain.Exceptions.IDs;

public sealed class InvalidMedicalNoteIdException() : BadRequestException("Medical note id cannot be empty.");