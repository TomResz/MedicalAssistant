﻿using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.DomainServices;
public interface IVisitService
{
	void AddRecommendation(Visit visit, UserId userId, MedicationRecommendation recommendation);
	void RemoveRecommendation(Visit visit, UserId user, MedicationRecommendationId recommendation);
}
