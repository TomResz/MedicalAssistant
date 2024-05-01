using MedicalAssist.Application.Dto;
using MedicalAssist.Domain.Entites;

namespace MedicalAssist.Infrastructure.DAL.QueryHandlers.Extensions;
internal static class VisitToVisitDto
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
		};
}
