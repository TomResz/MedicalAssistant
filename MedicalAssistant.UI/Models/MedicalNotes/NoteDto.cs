namespace MedicalAssistant.UI.Models.MedicalNotes;

public class NoteDto
{
    public Guid Id { get; set; }
    public string Note { get; set; }
    public string[] Tags { get; set; }
    public DateTime CreatedAt { get; set; }
}