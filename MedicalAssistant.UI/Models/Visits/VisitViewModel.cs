namespace MedicalAssistant.UI.Models.Visits;

public class VisitViewModel
{
	public string City { get; set; }
	public string PostalCode { get; set; }
	public string VisitType { get; set; }	
	public string Street { get; set; }
	public DateTime? Date { get; set; }
	public TimeSpan? Time { get; set; }
	public string DoctorName { get; set; }
	public string VisitDescription { get; set; }
	public TimeSpan? PredictedEndDate { get; set; }
}
