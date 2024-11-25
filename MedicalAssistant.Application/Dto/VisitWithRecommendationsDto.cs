namespace MedicalAssistant.Application.Dto;

public class VisitWithRecommendationsDto
{
    public DateTime Date { get; set; }
    public string DoctorName { get; set; }
    public string VisitDescription { get; set; }
    public string VisitType { get; set; }
    public List<RecommendationDto> Recommendations { get; set; }
}