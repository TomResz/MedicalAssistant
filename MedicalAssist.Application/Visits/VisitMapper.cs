﻿using MedicalAssist.Application.Dto;
using MedicalAssist.Domain.Entites;

namespace MedicalAssist.Application.Visits;
public static class VisitMapper
{
	public static VisitDto ToDto(this Visit visit)
	=> new VisitDto
	{
		Address = new Location
		{
			City = visit.Address.City,
			PostalCode = visit.Address.PostalCode,
			Street = visit.Address.Street,
		},
		Date = visit.Date.Value,
		DoctorName = visit.DoctorName.Value,
		VisitDescription = visit.VisitDescription.Value,
		VisitId = visit.Id,
		VisitType = visit.VisitType.Value,
		EndDate = visit.PredictedEndDate.Value,
	};
}
