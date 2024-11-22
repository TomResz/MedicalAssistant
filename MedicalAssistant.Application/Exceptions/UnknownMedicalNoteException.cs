using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;

public sealed class UnknownMedicalNoteException(Guid requestId)
    : BadRequestException($"Note with Id='{requestId}' does not exist.");