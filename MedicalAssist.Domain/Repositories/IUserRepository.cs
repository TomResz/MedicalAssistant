using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Repositories;
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(Email email,CancellationToken cancellationToken = default);
    Task<User?> GetByEmailWithExternalProviderAsync(Email email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailWithUserVerificationAsync(Email email,CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);
	Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);
	Task<User?> GetByIdWithUserVerificationAsync(UserId userId, CancellationToken cancellationToken = default);
	Task<User?> GetUserWithVisitsAsync(UserId userId, CancellationToken cancellationToken =default);
	Task<User?> GetByVerificationCodeAsync(CodeHash codeHash, CancellationToken cancellationToken = default);
	void Update(User user);
}
