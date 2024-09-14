namespace MedicalAssistant.UI.Models.Visits;

public static class EditVisitModelMapper
{
	public static EditVisitModel ToEditModel(this VisitViewModel model, Guid visitId)
	{
		var defaultTime = TimeSpan.FromHours(12);
		var defaultVisitTime = TimeSpan.FromMinutes(30);
		var date = model.Date!.Value.Add(model.Time ?? defaultTime);
		return new EditVisitModel()
		{
			City = model.City,
			PostalCode = model.PostalCode,
			Street = model.Street,
			Date = date.ToString("yyyy-MM-dd HH:mm"),
			DoctorName = model.DoctorName,
			VisitType = model.VisitType,
			VisitDescription = model.VisitDescription,
			PredictedEndDate = date.Add(model.PredictedEndDate ?? defaultVisitTime).ToString("yyyy-MM-dd HH:mm"),
			Id = visitId
		};
	}
}
