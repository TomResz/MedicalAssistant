namespace MedicalAssistant.Domain.Repositories;
public interface IRefreshTokenRepository
{
	Task<int> DeleteAsync(string refreshToken,CancellationToken cancellationToken);
}
