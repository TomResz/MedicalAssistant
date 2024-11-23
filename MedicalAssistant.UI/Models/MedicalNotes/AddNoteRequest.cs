namespace MedicalAssistant.UI.Models.MedicalNotes;

public class AddNoteRequest
{
    public string Note { get; set; }
    public string[] Tags { get; set; }
    public DateTime CreatedAt { get; set; }
}