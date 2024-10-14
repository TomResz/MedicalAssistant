using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.Medication;

public partial class AddMedicationRecommendationDialog
{
	private MudForm _form;
	private DateRange _dateRange { get; set; }

	[Parameter]
	public DateRange? DateRange { get; set; } = null;

	protected override void OnParametersSet()
	{
		if (DateRange is not null)
		{
			_dateRange = DateRange;
		}
	}

}
