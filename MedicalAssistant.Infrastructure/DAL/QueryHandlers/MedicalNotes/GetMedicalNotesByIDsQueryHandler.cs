using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicalNotes.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicalNotes;

internal sealed class GetMedicalNotesByIDsQueryHandler : IQueryHandler<GetMedicalNotesByIDsQuery,List<MedicalNoteDto>>
{
    private readonly IUserContext _userContext;
    private readonly MedicalAssistantDbContext _dbContext;
    
    public GetMedicalNotesByIDsQueryHandler(
        IUserContext userContext,
        MedicalAssistantDbContext dbContext)
    {
        _userContext = userContext;
        _dbContext = dbContext;
    }

    public async Task<List<MedicalNoteDto>> Handle(GetMedicalNotesByIDsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;
        
        var response = await _dbContext
            .MedicalNotes
            .Where(n => n.UserId == userId && request.Ids.Contains(n.Id))
            .Select(x => new MedicalNoteDto
            {
                Id = x.Id,
                CreatedAt = (DateTime)x.CreatedAt,
                Note = x.Note!,
                Tags = x.Tags,
            })
            .OrderBy(x=>x.CreatedAt)
            .ToListAsync(cancellationToken);
        
        return response;    
    }
}