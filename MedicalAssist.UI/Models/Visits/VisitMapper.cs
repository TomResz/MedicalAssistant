namespace MedicalAssist.UI.Models.Visits;

public static class VisitMapper
{
	public static CreateVisitVisitModel ToModel(this VisitViewModel viewModel)
	{
		var defaultTime = TimeSpan.FromHours(12);
		var defaultVisitTime = TimeSpan.FromMinutes(30);
		var date = viewModel.Date!.Value.Add(viewModel.Time ?? defaultTime);
		return new CreateVisitVisitModel()
		{
			City = viewModel.City,
			PostalCode = viewModel.PostalCode,
			Street = viewModel.Street,
			Date = date.ToString("yyyy-MM-dd HH:mm"),
			DoctorName = viewModel.DoctorName,
			VisitType = viewModel.VisitType,
			VisitDescription = viewModel.VisitDescription,
			PredictedEndDate = date.Add(viewModel.PredictedEndDate ?? defaultVisitTime).ToString("yyyy-MM-dd HH:mm")
		};
	}


}

