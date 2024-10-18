using MedicalAssistant.UI.Components.Medication;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Pages.Dashboard.Medication;

public partial class MedicationSchedulerPage
{
	[Inject]
	public IMedicationService MedicationService { get; set; }

	private bool _loading = true;

	private List<MedicationDto> _items = [];
	protected override async Task OnInitializedAsync()
	{
		var response = await MedicationService.GetAll();
		if(response.IsSuccess)
		{
			_items = [.. response.Value!];
		}
		_loading = false;
	}
}
