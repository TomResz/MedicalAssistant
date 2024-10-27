﻿using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;

public sealed record GetMedicationRecommendationByDateQuery(
    DateTime Date) : IRequest<IEnumerable<MedicationRecommendationDto>>;