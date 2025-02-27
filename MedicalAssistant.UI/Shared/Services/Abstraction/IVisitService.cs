﻿using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;
public interface IVisitService
{
    Task<Response<List<VisitDto>>> GetAllVisits(string? direction = null);
	Task<Response<VisitDto>> Add(CreateVisitVisitModel model);
	Task<Response.Base.Response> Delete(Guid visitId);
	Task<Response<VisitDto>> Edit(EditVisitModel editVisitModel);
	Task<Response.Base.Response<VisitDto>> Get(Guid visitId);
	Task<Response<List<VisitDto>>> GetByWeek(DateTime dateTime);
	Task<Response<List<VisitDto>>> GetCompleted(DateTime dateTime);
	Task<Response<List<VisitDto>>> GetByDate(DateTime date);
	Task<Response<List<VisitDto>>> GetBySearchTerm(string searchTerm,string? direction = null);
}