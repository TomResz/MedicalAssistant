using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;

public class UnknownRecommendationException(Guid recommendationId) : BadRequestException($"Recommendation with given id='{recommendationId}' was not found.");

public sealed class InvalidStartDateException() : BadRequestException("End date cannot be earlier than start date.");
public sealed class InvalidEndDateException() : BadRequestException($"Start date is greater than end date.");