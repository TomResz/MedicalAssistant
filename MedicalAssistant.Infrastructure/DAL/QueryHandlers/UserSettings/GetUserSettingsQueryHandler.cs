using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.UserSettings.Queries;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssistant.Infrastructure.DAL.QueryHandlers.UserSettings;
internal sealed class GetUserSettingsQueryHandler
	: IQueryHandler<GetUserSettingsQuery, SettingsDto>
{
	private readonly MedicalAssistantDbContext _context;
	private readonly IUserContext _userContext;
	public GetUserSettingsQueryHandler(
		MedicalAssistantDbContext context,
		IUserContext userContext)
	{
		_context = context;
		_userContext = userContext;
	}

	public async Task<SettingsDto> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
	{
		var userId = _userContext.GetUserId;

		var response = await _context
			.UserSettings
			.Where(x => x.UserId == userId)
			.Select(x => new SettingsDto
			{
				EmailNotificationEnabled = x.EmailNotificationEnabled,
				NotificationLanguage = x.NotificationLanguage.ToString(),
				NotificationsEnabled = x.NotificationsEnabled,
				VisitNotificationEnabled = x.VisitNotificationEnabled,
			})
			.FirstOrDefaultAsync(cancellationToken);

		return response ?? throw new UserNotFoundException();
	}
}
