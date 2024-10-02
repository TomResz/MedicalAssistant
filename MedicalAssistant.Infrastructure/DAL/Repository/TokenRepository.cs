using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.ComplexTypes;

namespace MedicalAssistant.Infrastructure.DAL.Repository;

internal sealed class TokenRepository : ITokenRepository
{
	private readonly MedicalAssistDbContext _context;

	public TokenRepository(MedicalAssistDbContext context)
	{
		_context = context;
	}

	public void Add(TokenHolder tokenHolder)
	{
		_context.TokenHolders.Add(tokenHolder);
	}
}
