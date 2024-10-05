using MedicalAssistant.UI.Models.Visits;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Components.Visits;

public partial class VisitItem
{
	[Parameter]
	public VisitDto Visit { get; set; }

	[Inject]
	public NavigationManager Navigation { get; set; }

	private void ShowDetails()
	{
		Navigation.NavigateTo($"/visit/{Visit.Id}");
	}
}
