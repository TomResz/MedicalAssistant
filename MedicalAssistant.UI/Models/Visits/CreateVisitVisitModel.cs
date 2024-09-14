namespace MedicalAssistant.UI.Models.Visits;

public class CreateVisitVisitModel
{
	public string City { get; set; }

	public string PostalCode { get; set; }

	public string Street { get; set; }
	public string Date { get; set; }

	public string DoctorName { get; set; }

	public string VisitType { get; set; }

	public string VisitDescription { get; set; }

	public string PredictedEndDate { get; set; }
}

