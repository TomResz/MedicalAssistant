﻿namespace MedicalAssistant.Application.Dto.Mappers;
public static class AttachmentToDtoMapper
{
	public static AttachmentDto ToDto(this Domain.Entites.Attachment attachment) 
		=> new AttachmentDto
	{
		Id = attachment.Id,
		Content = attachment.Content,
		FileExtension = attachment.Extension,
		Name = attachment.Name,
	};
}