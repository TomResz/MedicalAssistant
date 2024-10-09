using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using System.Web;

namespace MedicalAssistant.UI.Pages.Dashboard.NotificationPages;

public partial class UpcomingVisitNotifications
{
	private List<VisitNotificationWithDetailsModel> _notifications = new();

	private int _currentPage;
	private int _pageSize = 5;
	private int _pageCount = 0;
	private int _totalItemCount = 0;
	private readonly List<int> _pageSizes = [5, 10, 15, 25, 50, 100];
	private bool _loading = false;
	private Dictionary<DateTime, string> _formattedDates = [];
	private const string RoutePrefix = "notification/visit";

	[Inject] public NavigationManager NavigationManager { get; set; }
	[Inject] public IVisitNotificationService NotificationService { get; set; }
	[Inject] public ILocalTimeProvider TimeProvider { get; set; }

	protected override async Task OnInitializedAsync()
	{
		_loading = true;
		(_currentPage, _pageSize) = SetOrGetPageQueryParameters(RoutePrefix, NavigationManager);
		await LoadData();
		_loading = false;
	}

	private async Task LoadData()
	{
		Response<PagedList<VisitNotificationWithDetailsModel>> response = await NotificationService.GetPage(_currentPage, _pageSize);
		if (response.IsSuccess)
		{
			_notifications = response.Value!.Items;
			_pageCount = response.Value!.PageTotalCount;
			_totalItemCount = response.Value!.TotalCount;
		}
		foreach (var item in _notifications) 
		{
            Console.WriteLine($"{item.Id}");
		}
		foreach (var notification in _notifications)
		{
			if (!_formattedDates.ContainsKey(notification.ScheduledDateUtc))
			{
				_formattedDates[notification.ScheduledDateUtc] = await FromUtc(notification.ScheduledDateUtc);
			}
		}

		await InvokeAsync(StateHasChanged);
	}
	private async Task OnPageChanged(int newPage)
	{
		_currentPage = newPage;

		var newUrl = $"/{RoutePrefix}?page={_currentPage}&pageSize={_pageSize}";
		NavigationManager.NavigateTo(newUrl);

		await LoadData();
	}

	private (int, int) SetOrGetPageQueryParameters(string routePrefix, NavigationManager navigation)
	{
		var uri = navigation.ToAbsoluteUri(navigation.Uri);

		var query = HttpUtility.ParseQueryString(uri.Query);

		var pageKey = query.AllKeys.FirstOrDefault(k => string.Equals(k, "page", StringComparison.OrdinalIgnoreCase));
		var pageSizeKey = query.AllKeys.FirstOrDefault(k => string.Equals(k, "pageSize", StringComparison.OrdinalIgnoreCase));
		int page = 1, size = 1;
		bool pageParsed = pageKey != null && int.TryParse(query[pageKey], out page);
		bool sizeParsed = pageSizeKey != null && int.TryParse(query[pageSizeKey], out size);

		if (!pageParsed || !sizeParsed)
		{
			_currentPage = pageParsed ? page : 1;
			_pageSize = sizeParsed ? size : 10;


			var newUrl = $"/{routePrefix}?page={_currentPage}&pageSize={_pageSize}";

			navigation.NavigateTo(newUrl);
			StateHasChanged();
		}
		return (page, size);
	}

	private async Task OnPageSizeChanged(int newCount)
	{
		Console.WriteLine(newCount);
		_pageSize = newCount;
		var newUrl = $"/{RoutePrefix}?page={_currentPage}&pageSize={_pageSize}";
		NavigationManager.NavigateTo(newUrl);
		await LoadData();
	}

	private async Task<string> FromUtc(DateTime dateUtc)
		=> (await TimeProvider.FromUtcToLocal(dateUtc)).ToString("HH:mm dd.MM.yyyy");


	private async Task Delete(Guid id)
	{
		var response = await NotificationService.Delete(id);
		if (response.IsSuccess) 
		{
			var item = _notifications.First(x=>x.Id == id);
			_notifications.Remove(item);
			StateHasChanged();
		}
	}
}
