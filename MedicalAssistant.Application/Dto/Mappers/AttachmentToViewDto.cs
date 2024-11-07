namespace MedicalAssistant.Application.Dto.Mappers;
public static class AttachmentToViewDto
{
	public static AttachmentViewDto ToViewDto(this Domain.Entities.Attachment attachment) 
		=> new AttachmentViewDto
	{
		FileName = attachment.Name,
		Id = attachment.Id,
	};
}
