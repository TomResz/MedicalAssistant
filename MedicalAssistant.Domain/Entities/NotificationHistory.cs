using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entities;
public class NotificationHistory
{
	public NotificationHistoryId Id { get; private set; }
	public UserId UserId { get; private set; }
	public ContentJson ContentJson { get; private set; }
	public NotificationHistoryType Type { get; private set; }
	public Date PublishedDate { get; private set; }
	public bool WasRead { get; private set; }
	public Date? DateOfRead { get; private set; }

	// EF core
	public User User { get; private set; }
	protected NotificationHistory()
	{
	}

	private NotificationHistory(
		NotificationHistoryId id,
		UserId userId,
		ContentJson contentJson,
		Date publishedDate,
		bool wasRead,
		NotificationHistoryType type,
		Date? dateOfRead)
	{
		Id = id;
		UserId = userId;
		ContentJson = contentJson;
		PublishedDate = publishedDate;
		WasRead = wasRead;
		DateOfRead = dateOfRead;
		Type = type;
	}
	public bool MarkAsRead()
	{
		WasRead = true;
		return WasRead;
	}
	public static NotificationHistory Create(
		UserId userId,
		ContentJson contentJson,
		NotificationHistoryType type,
		Date publishedDate)
	{
		return new(
		Guid.NewGuid(),
		userId,
		 contentJson,
		 publishedDate,
		 false,
		 type,
		 null);
	}
}
