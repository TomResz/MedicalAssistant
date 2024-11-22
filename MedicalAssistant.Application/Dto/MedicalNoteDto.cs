namespace MedicalAssistant.Application.Dto;

public class MedicalNoteDto
{
    public Guid Id { get; set; }
    public string Note { get; set; }
    public DateTime  CreatedAt { get; set; }
    public string[] Tags { get; set; }
}