using System.Text.Json.Serialization;

namespace MedicalAssist.UI.Models.Visits;

public class VisitModel
{
	[JsonPropertyName("visitId")]
	public Guid Id { get; set; }
	
	[JsonPropertyName("address")]
	public LocationModel Address { get; set; }
	
	[JsonPropertyName("date")]
	public DateTime Date { get; set; }
	public DateTime End => Date.AddHours(2);

	[JsonPropertyName("doctorName")]
	public string DoctorName { get; set; }

	[JsonPropertyName("visitDescription")]
	public string VisitDescription { get; set; }

	[JsonPropertyName("visitType")]
	public string VisitType { get; set; }
}

public class LocationModel
{
	[JsonPropertyName("city")]
	public string City { get; set; }

	[JsonPropertyName("postalCode")]
	public string PostalCode { get; set; }

	[JsonPropertyName("street")]
	public string Street { get; set; }
}