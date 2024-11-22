using Dapper;
using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicalNotes.Queries;
using MedicalAssistant.Infrastructure.DAL.Dapper;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicalNotes;

internal sealed class GetTagsOfNotesQueryHandler 
    : IRequestHandler<GetTagsOfNotesQuery,IEnumerable<NoteTagDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IUserContext _userContext;
    
    public GetTagsOfNotesQueryHandler(
        IUserContext userContext, 
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _userContext = userContext;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IEnumerable<NoteTagDto>> Handle(GetTagsOfNotesQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId.Value;

        using var connection = _sqlConnectionFactory.Create();

        var response = await connection.QueryAsync<NoteTagDto>($"""
         SELECT 
             tag AS "Tag", 
             COUNT(*) AS "Count"
         FROM public."MedicalNotes" m, 
              LATERAL UNNEST(m."Tags") AS tag
         WHERE m."UserId" = @userId
         GROUP BY tag
         ORDER BY COUNT(*) DESC
         """,new{userId = userId});
        
        return response;
    }
}