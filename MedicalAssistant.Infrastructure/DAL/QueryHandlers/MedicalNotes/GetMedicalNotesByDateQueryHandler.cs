using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicalNotes.Queries;
using MedicalAssistant.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.MedicalNotes;

internal sealed class GetMedicalNotesByDateQueryHandler : IRequestHandler<GetMedicalNotesByDateQuery,IEnumerable<MedicalNoteDto>>
{
    private readonly MedicalAssistantDbContext _context;
    private readonly IUserContext _userContext;

    public GetMedicalNotesByDateQueryHandler(
        MedicalAssistantDbContext context,
        IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<IEnumerable<MedicalNoteDto>> Handle(GetMedicalNotesByDateQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId;
        Date startDate = request.Date;
        DateTime endDate = request.Date.AddDays(1).AddTicks(-1);

        var response = await _context.MedicalNotes
            .Where(n => n.UserId == userId && 
                        startDate <= n.CreatedAt &&
                        endDate >= n.CreatedAt)
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