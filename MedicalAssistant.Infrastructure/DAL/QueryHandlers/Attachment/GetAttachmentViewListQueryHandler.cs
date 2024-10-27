using Dapper;
using MediatR;
using MedicalAssistant.Application.Attachment.Queries;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Infrastructure.DAL.Dapper;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.Attachment;
internal sealed class GetAttachmentViewListQueryHandler
	: IRequestHandler<GetAttachmentViewListQuery, IEnumerable<AttachmentViewDto>>
{
	private readonly ISqlConnectionFactory _connectionFactory;
	public GetAttachmentViewListQueryHandler(
		ISqlConnectionFactory connectionFactory)
	{
		_connectionFactory = connectionFactory;
	}


	public async Task<IEnumerable<AttachmentViewDto>> Handle(GetAttachmentViewListQuery request, CancellationToken cancellationToken)
	{
		const string sql = $"""	
			SELECT 
				a."Id" as {nameof(AttachmentViewDto.Id)},
				a."Name" as {nameof(AttachmentViewDto.FileName)}
				FROM "Attachments" as a
				WHERE a."VisitId" = @visitId
			""";

		using var connection = _connectionFactory.Create();

		var response = await connection.QueryAsync<AttachmentViewDto>(
			sql: sql
			, param: new { visitId = request.VisitId });

		return response;
	}
}
