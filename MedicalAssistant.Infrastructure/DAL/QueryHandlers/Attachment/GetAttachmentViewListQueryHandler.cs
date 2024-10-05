using MediatR;
using MedicalAssistant.Application.Attachment.Queries;
using MedicalAssistant.Application.Dto;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Attachment;
internal sealed class GetAttachmentViewListQueryHandler
	: IRequestHandler<GetAttachmentViewListQuery, IEnumerable<AttachmentViewDto>>
{
	private readonly MedicalAssistantDbContext _context;

	public GetAttachmentViewListQueryHandler(MedicalAssistantDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<AttachmentViewDto>> Handle(GetAttachmentViewListQuery request, CancellationToken cancellationToken)
	{
		var response = await _context
			.Database
			.SqlQuery<AttachmentViewDto>($"""
				SELECT a."Id" as Id,
				a."Name" as FileName
				FROM "Attachments" as a
				WHERE a."VisitId" = {request.VisitId}
			""")
			.ToListAsync(cancellationToken);

		return response;
	}
}
