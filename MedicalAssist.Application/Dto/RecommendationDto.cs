namespace MedicalAssist.Application.Dto;
public class RecommendationDto
{
    public Guid Id { get; set; }
    public string? ExtraNote { get; set; }
    public DateTime CreatedAt { get; set; }
    public string MedicineName { get; set; }
    public int MedicineQuantity { get; set; }
    public string MedicineTimeOfDay { get; set; }
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
}
