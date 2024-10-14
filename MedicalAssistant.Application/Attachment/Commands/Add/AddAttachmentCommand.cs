﻿using MediatR;
using MedicalAssistant.Application.Dto;
using Microsoft.AspNetCore.Http;

namespace MedicalAssistant.Application.Attachment.Commands.Add;
public sealed record AddAttachmentCommand(
	Guid VisitId,
	IFormFile File) : IRequest<AttachmentViewDto>;