using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;

public sealed class UnknownDiseaseStageException(Guid requestId)
    : BadRequestException($"Stage with Id='{requestId}' was not found.");