namespace MedicalAssist.API.Models;

public class AddRecommendationModel
{
    public string? ExtraNote { get; set; } = null;
    public string MedicineName { get; set; }
    public int Quantity { get; set; }
    public string TimeOfDay { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
