using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicalNotes.Queries;
using MedicalAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicalNotes;

internal sealed class GetMedicalNotesByTermAndTagsQueryHandler 
    : IQueryHandler<GetMedicalNotesByTermAndTagsQuery,IEnumerable<MedicalNoteDto>>
{
    private readonly IUserContext _userContext;
    private readonly MedicalAssistantDbContext _context;
    
    public GetMedicalNotesByTermAndTagsQueryHandler(
        IUserContext userContext,
        MedicalAssistantDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<IEnumerable<MedicalNoteDto>> Handle(
        GetMedicalNotesByTermAndTagsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;

        IQueryable<MedicalNote> query = _context
                .MedicalNotes
                .Where(x => x.UserId == userId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var formattedQuery = request.SearchTerm.ToTsQuery();
            
            query = query.Where(x => EF.Functions.ToTsVector("english", x.Note! )
                                .Matches(EF.Functions.ToTsQuery("english", formattedQuery)));   
        }

        if (request.Tags is not null && 
            request.Tags.Length != 0)
        {
            var tags = MedicalNote.NormalizeTags(request.Tags);
            query = query.Where(x => x.Tags.Any(y => tags.Contains(y)));
        }
        
        var result = await query
            .Select(x=> new MedicalNoteDto
            {
                Id = x.Id,
                Note = x.Note!,
                Tags = x.Tags,
                CreatedAt = (DateTime)x.CreatedAt,
            })
            .OrderBy(x=>x.CreatedAt)
            .ToListAsync(cancellationToken);

        return result;
    }
}