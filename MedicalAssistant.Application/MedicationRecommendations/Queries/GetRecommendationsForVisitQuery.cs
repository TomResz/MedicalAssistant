﻿using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;
public sealed record GetRecommendationsForVisitQuery(
    Guid VisitId) : IQuery<IEnumerable<RecommendationDto>>;
