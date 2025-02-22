﻿using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Domain.Repositories;
public interface INotificationHistoryRepository
{
	void Add(NotificationHistory notificationHistory);
	Task MarkAsReadRange(List<Guid> ids,Date dateUtc);	
}
