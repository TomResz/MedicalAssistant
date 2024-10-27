﻿using MedicalAssistant.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.Repository;
internal sealed class RefreshTokenRepository : IRefreshTokenRepository
{
	private readonly MedicalAssistantDbContext _context;

	public RefreshTokenRepository(MedicalAssistantDbContext context)
	{
		_context = context;
	}

	public async Task<int> DeleteAsync(string refreshToken, CancellationToken cancellationToken) 
		=> await _context.TokenHolders
			.Where(x => x.RefreshToken == refreshToken)
			.ExecuteDeleteAsync(cancellationToken);
}