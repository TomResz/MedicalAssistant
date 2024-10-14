using MedicalAssistant.UI.Components.Medication;

namespace MedicalAssistant.UI.Pages.Dashboard.Medication;

public partial class MedicationSchedulerPage
{
	private bool _loading = true;

	private readonly List<MedicationDto> _items = [];
	protected override async Task OnInitializedAsync()
	{
		await Task.Delay(3_000);
		_loading = false;
	}
}
