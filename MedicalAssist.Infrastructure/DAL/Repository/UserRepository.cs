using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Repositories;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssist.Infrastructure.DAL.Repository;
internal sealed class UserRepository : IUserRepository
{
    private readonly MedicalAssistDbContext _context;
    public UserRepository(MedicalAssistDbContext context) => _context = context;

    public async Task AddAsync(User user, CancellationToken cancellationToken = default) 
        => await _context
        .AddAsync(user,cancellationToken);

    public async Task<User?> GetByVerificationCodeAsync(CodeHash codeHash, CancellationToken cancellationToken = default)
        => await _context
        .Users
        .Include(x => x.UserVerification)
        .FirstOrDefaultAsync(x => x.UserVerification.CodeHash == codeHash,cancellationToken);

	public async Task<User?> GetByEmailAsync(Domain.ValueObjects.Email email, CancellationToken cancellationToken = default)
        => await _context
        .Users
        .FirstOrDefaultAsync( x => x.Email == email,cancellationToken);

	public async Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default)
        => await _context
        .Users
        .FirstOrDefaultAsync(x=> x.Id == userId, cancellationToken);

	public async Task<User?> GetUserWithVisitsAsync(UserId userId, CancellationToken cancellationToken = default) 
        => await _context
        .Users
        .Include(x => x.Visits)
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == userId,cancellationToken);

	public async Task<bool> IsEmailUniqueAsync(Domain.ValueObjects.Email email, CancellationToken cancellationToken = default)
        => !await _context
        .Users
        .AnyAsync(x => x.Email == email,cancellationToken);

	public void Update(User user)
        => _context
           .Update(user);

	public async Task<User?> GetByEmailWithUserVerificationAsync(Domain.ValueObjects.Email email, CancellationToken cancellationToken = default)
		=> await _context
		.Users
        .Include(x=> x.UserVerification)
		.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

	public async Task<User?> GetByIdWithUserVerificationAsync(UserId userId, CancellationToken cancellationToken = default)
	    => await _context
		.Users
        .Include(x=>x.UserVerification)
		.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

}
