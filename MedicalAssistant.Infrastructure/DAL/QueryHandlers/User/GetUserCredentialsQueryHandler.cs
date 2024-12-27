using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.User.Queries;
using MedicalAssistant.Domain.ValueObjects.IDs;
using MedicalAssistant.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.User;
internal sealed class GetUserCredentialsQueryHandler
	: IQueryHandler<GetUserCredentialsQuery, UserCredentialsDto>
{
	private readonly IUserContext _userContext;
	private readonly MedicalAssistantDbContext _context;
	public GetUserCredentialsQueryHandler(IUserContext userContext, MedicalAssistantDbContext context)
	{
		_userContext = userContext;
		_context = context;
	}

	public async Task<UserCredentialsDto> Handle(GetUserCredentialsQuery request, CancellationToken cancellationToken)
	{
		UserId userId = _userContext.GetUserId;

		var response = await _context
			.Users
			.AsNoTracking()
			.Where(x=> x.Id == userId)
			.Select(x => x.ToUserCredentialsDto())
			.FirstOrDefaultAsync();
		return response ?? throw new UserNotFoundException();
	}
}
