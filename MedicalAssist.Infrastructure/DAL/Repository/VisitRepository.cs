using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Infrastructure.DAL.Repository;
internal sealed class VisitRepository : IVisitRepository
{
	private readonly MedicalAssistDbContext _context;

	public VisitRepository(MedicalAssistDbContext context) => _context = context;

	public void Add(Visit visit)
		=> _context.Visits.Add(visit);
}
