namespace MedicalAssistant.UI.Models.MedicalNotes;

public class EditNoteRequest
{
    public Guid Id { get; set; }
    public string Note { get; set; }
    public string[] Tags { get; set; }
}