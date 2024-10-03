namespace MedicalAssistant.Domain.Entites;
public class Attachment
{
    public Guid Id { get; set; }
    public string Extension { get; set; }
    public string Title { get; set; }
    public int Length { get; set; }
    public byte[] Content { get; set; }
}
