using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Application.Tests.ObjectFactories;
public static class VisitFactory
{
	public static Visit Create(
		UserId userId,
		Address? address = null,
		Date? date = null,
		DoctorName? doctorName = null,
		VisitDescription? visitDescription = null,
		VisitType? visitType = null,
		Date? predictedEndDate = null)
	{
		address ??= new Address("default street", "default city", "00-000");
		date ??= DateTime.Now;
		doctorName ??= new DoctorName("Doctor");
		visitDescription ??= new VisitDescription("Default description");
		visitType ??= new("Surgeon"); 
		predictedEndDate ??= date.Value.AddHours(1);

		return Visit.Create(
			userId,
			address,
			date,
			doctorName,
			visitDescription,
			  visitType,
			   predictedEndDate);
	}
}
