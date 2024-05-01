using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Exceptions;
using MedicalAssist.Application.User.Queries;
using MedicalAssist.Domain.ValueObjects.IDs;
using MedicalAssist.Infrastructure.DAL.QueryHandlers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssist.Infrastructure.DAL.QueryHandlers.User;
internal sealed class GetUserCredentialsQueryHandler
	: IRequestHandler<GetUserCredentialsQuery, UserCredentialsDto>
{
	private readonly IUserContext _userContext;
	private readonly MedicalAssistDbContext _context;
	public GetUserCredentialsQueryHandler(IUserContext userContext, MedicalAssistDbContext context)
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
			.Where(x=> x.Id.Value == userId.Value)
			.Select(x => x.ToUserCredentialsDto())
			.FirstOrDefaultAsync();
		return response ?? throw new UserNotFoundException();
	}
}
