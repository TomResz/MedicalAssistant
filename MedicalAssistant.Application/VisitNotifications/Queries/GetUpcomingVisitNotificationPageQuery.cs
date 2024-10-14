﻿using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Pagination;

namespace MedicalAssistant.Application.VisitNotifications.Queries;
public sealed record GetUpcomingVisitNotificationPageQuery(
	int Page,
	int PageSize) : IRequest<PagedList<UpcomingVisitNotificationDto>>;