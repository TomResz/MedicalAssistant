﻿using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;

public sealed record GetByIdsMedicationRecommendationsQuery(List<Guid> IDs) : IQuery<IEnumerable<MedicationRecommendationDto>>;