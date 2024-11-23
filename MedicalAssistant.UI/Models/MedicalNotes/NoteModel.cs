namespace MedicalAssistant.UI.Models.MedicalNotes;

public class NoteModel
{
    public string Note { get; set; } = string.Empty;
    public string CurrentTag { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
}