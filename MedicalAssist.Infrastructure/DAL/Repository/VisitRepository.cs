using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssist.Infrastructure.DAL.Repository;
internal sealed class VisitRepository : IVisitRepository
{
	private readonly MedicalAssistDbContext _context;

	public VisitRepository(MedicalAssistDbContext context) => _context = context;

	public void Add(Visit visit)
		=> _context.Visits.Add(visit);

	public async Task<Visit?> GetByIdAsync(VisitId visitId, CancellationToken cancellationToken)
		=> await _context
		.Visits
		.Include(x => x.Recommendations)
		.FirstOrDefaultAsync(x => x.Id == visitId, cancellationToken);

	public void Update(Visit visit)
		=> _context
		.Visits
		.Update(visit);
}
