namespace MedicalAssistant.Application.Dto;

public class AttachmentDto
{
    public Guid Id { get; set; }
    public byte[] Content { get; set; }
    public string Name { get; set; }
    public string FileExtension { get; set; }
}
