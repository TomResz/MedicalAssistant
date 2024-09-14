namespace MedicalAssistant.Application.Dto;
public class VisitDto
{
    public Guid VisitId { get; set; }
	public Location Address { get;  set; }
	public DateTime Date { get; set; }
	public DateTime EndDate { get; set; }	
	public string DoctorName { get; set; }
	public string VisitDescription { get; set; }
	public string VisitType { get; set; }
}
