using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Pages.Dashboard.VisitPages;

public partial class AllVisitsPage
{
	private List<VisitDto> _visits = [];
	private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
	private string SearchTerm { get; set; }

	[Parameter]
    [SupplyParameterFromQuery(Name ="direction")]
    public string? Direction { get; set; }

    [Inject]
	private IVisitService VisitService { get; set; }


	[Inject]
	public NavigationManager Navigation { get; set; }	

	public string SelectedSortingDirection { get; set; } = Translations.Ascending;




    protected override async Task OnInitializedAsync()
	{

		string directionStr = Direction == Translations.Ascending ? "asc": "desc";
		var resposne = await VisitService.GetAllVisits(directionStr);
		if (resposne.IsSuccess)
		{
			_visits = resposne.Value!;
		}
		Navigation.NavigateTo($"/visits/all?direction={directionStr}");

	}

	private async Task DirectionChanged(string value)
	{
		var directionStr = value == Translations.Ascending ? "asc" : "desc";
		SelectedSortingDirection = value;
		Navigation.NavigateTo($"/visits/all?direction={Direction}");


		if(!string.IsNullOrEmpty(SearchTerm))
		{
			var response = await VisitService.GetBySearchTerm(SearchTerm, Direction);
			if (response.IsSuccess)
			{
				_visits = response.Value!;
				await InvokeAsync(StateHasChanged);
			}
			return;
		}
		else
		{
			var resposne = await VisitService.GetAllVisits(directionStr);
			if (resposne.IsSuccess)
			{
				_visits = resposne.Value!;
				await InvokeAsync(StateHasChanged);
			}
		}

	}

	private async Task SearchVisits(string text)
	{
		if (text is null)
		{
			return;
		}

		_cancellationTokenSource.Cancel();
		_cancellationTokenSource = new CancellationTokenSource();

		await Task.Delay(300);

		var token = _cancellationTokenSource.Token;


		Console.WriteLine(Direction);

		if (string.IsNullOrEmpty(text))
		{
			var resposne = await VisitService.GetAllVisits();
			if (resposne.IsSuccess)
			{
				_visits = resposne.Value!;
				await InvokeAsync(StateHasChanged);
			}
			return;
		}


		if (!token.IsCancellationRequested)
		{
			var response = await VisitService.GetBySearchTerm(text,Direction);
			if (response.IsSuccess)
			{
				_visits = response.Value!;
				await InvokeAsync(StateHasChanged);
			}
		}
	}
}
