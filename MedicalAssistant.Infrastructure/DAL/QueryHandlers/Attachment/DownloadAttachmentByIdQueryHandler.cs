using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Attachment.Queries;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Attachment;
internal sealed class DownloadAttachmentByIdQueryHandler
	: IQueryHandler<DownloadAttachmentByIdQuery, AttachmentDto?>
{
	private readonly MedicalAssistantDbContext _context;

	public DownloadAttachmentByIdQueryHandler(MedicalAssistantDbContext context)
	{
		_context = context;
	}

	public async Task<AttachmentDto?> Handle(DownloadAttachmentByIdQuery request, CancellationToken cancellationToken)
	{
		return await _context
			.Attachments
			.Where(x => x.Id == new AttachmentId(request.Id))
			.Select(x => x.ToDto())
			.AsNoTracking()
			.FirstOrDefaultAsync(cancellationToken);
	}
}
