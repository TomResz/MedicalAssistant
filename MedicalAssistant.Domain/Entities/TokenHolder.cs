using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.ComplexTypes;
public class TokenHolder
{
    public TokenHolderId Id { get; set; }
    public RefreshToken RefreshToken { get; private set; }
	public Date RefreshTokenExpirationUtc { get; private set; }
	public UserId UserId { get; private set; }

	private TokenHolder(
		TokenHolderId id,
		RefreshToken refreshToken, 
		Date refreshTokenExpirationUtc,
		UserId userId)
	{
		Id = id;
		RefreshToken = refreshToken;
		RefreshTokenExpirationUtc = refreshTokenExpirationUtc;
		UserId = userId;
	}

	private TokenHolder()
	{
	}

	public static TokenHolder Create(RefreshToken RefreshToken, Date ExpirationUtc,UserId userId)
		=> new(Guid.NewGuid(),RefreshToken, ExpirationUtc,userId);

}
