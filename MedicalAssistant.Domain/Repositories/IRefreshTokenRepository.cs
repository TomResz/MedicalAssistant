using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Repositories;
public interface IRefreshTokenRepository
{
	Task<int> DeleteAsync(string refreshToken,CancellationToken cancellationToken);
	Task<int> DeleteAsync(UserId userId,CancellationToken cancellationToken);
}
