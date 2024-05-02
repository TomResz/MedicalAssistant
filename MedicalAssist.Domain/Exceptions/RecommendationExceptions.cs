﻿using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;

public class UnknownRecommendationException : BadRequestException
{
    public UnknownRecommendationException(Guid recommendationId) : base($"Recommendation with given id='{recommendationId}' was not found.")
    {
        
    }
}