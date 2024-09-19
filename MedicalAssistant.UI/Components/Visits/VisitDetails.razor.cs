using MedicalAssistant.UI.Models.Visits;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Components.Visits;

public partial class VisitDetails
{
	[Parameter]
	public VisitDto Visit { get; set; }
}
