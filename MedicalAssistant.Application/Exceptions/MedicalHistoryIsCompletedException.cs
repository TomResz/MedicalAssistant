using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;

public sealed class MedicalHistoryIsCompletedException()
    : BadRequestException("Cannot edit or add stage because history is completed.");