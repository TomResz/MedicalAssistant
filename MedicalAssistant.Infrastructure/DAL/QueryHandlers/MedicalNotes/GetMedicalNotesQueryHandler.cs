using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicalNotes.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicalNotes;

internal sealed class GetMedicalNotesQueryHandler
    : IRequestHandler<GetMedicalNotesQuery,IEnumerable<MedicalNoteDto>>
{
    private readonly MedicalAssistantDbContext _context;
    private readonly IUserContext _userContext;
    public GetMedicalNotesQueryHandler(
        MedicalAssistantDbContext context,
        IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<IEnumerable<MedicalNoteDto>> Handle(GetMedicalNotesQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;

        var response = await _context.MedicalNotes
            .Where(n => n.UserId == userId)
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