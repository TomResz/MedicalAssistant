using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;

public sealed class MedicalHistoryVisitDateMismatchException()
    : BadRequestException("Medical history date must be same as visit date.");