﻿using MediatR;
using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.Visits.Queries;
public sealed record GetVisitsByDayQuery(
    DateTime Day) : IRequest<IEnumerable<VisitDto>>;