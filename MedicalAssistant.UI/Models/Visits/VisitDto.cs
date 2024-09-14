using System.Text.Json.Serialization;

namespace MedicalAssistant.UI.Models.Visits;

public class VisitDto
{
	[JsonPropertyName("visitId")]
	public Guid Id { get; set; }
	
	[JsonPropertyName("address")]
	public LocationDto Address { get; set; }
	
	[JsonPropertyName("date")]
	public DateTime Date { get; set; }

	[JsonPropertyName("endDate")]
	public DateTime End {  get; set; }

	[JsonPropertyName("doctorName")]
	public string DoctorName { get; set; }

	[JsonPropertyName("visitDescription")]
	public string VisitDescription { get; set; }

	[JsonPropertyName("visitType")]
	public string VisitType { get; set; }
}

public class LocationDto
{
	[JsonPropertyName("city")]
	public string City { get; set; }

	[JsonPropertyName("postalCode")]
	public string PostalCode { get; set; }

	[JsonPropertyName("street")]
	public string Street { get; set; }
}