﻿using MediatR;
using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.Visits.Queries;
public sealed record GetAllVisitsQuery() : IRequest<IEnumerable<VisitDto>>;