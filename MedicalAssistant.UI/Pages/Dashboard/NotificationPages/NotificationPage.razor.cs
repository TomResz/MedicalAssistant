using MedicalAssistant.UI.Components.AppBar.Notifications;
using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using System.Web;

namespace MedicalAssistant.UI.Pages.Dashboard.NotificationPages;

public partial class NotificationPage
{
	private List<NotificationModel> _pagedNotifications = [];
	private NotificationModel _selectedNotification;

	private int _currentPage;
	private int _pageSize = 5;
	private int _pageCount = 0;
	private int _totalItemCount = 0;
	private readonly List<int> _pageSizes = [5, 10, 15, 25, 50, 100];
	private bool _loading = false;
	private Dictionary<DateTime, string> _formattedDates = new();

	[Inject] public NavigationManager NavigationManager { get; set; }
	[Inject] public INotificationService NotificationService { get; set; }
	[Inject] public ILocalTimeProvider TimeProvider { get; set; }
	protected override async Task OnInitializedAsync()
	{
		_loading = true;
		(_currentPage, _pageSize) = SetOrGetPageQueryParameters();
		await LoadData();
		_loading = false;
	}

	private async Task LoadData()
	{
		Response<PagedList<NotificationModel>> response = await NotificationService.GetPage(_currentPage, _pageSize);
		if (response.IsSuccess)
		{
			_pagedNotifications = response.Value!.Items;
			_pageCount = response.Value!.PageTotalCount;
			_totalItemCount = response.Value!.TotalCount;
		}
		foreach (var notification in _pagedNotifications)
		{
			if (!_formattedDates.ContainsKey(notification.PublishedDateUtc))
			{
				_formattedDates[notification.PublishedDateUtc] = await FromUtc(notification.PublishedDateUtc);
			}
		}
		await InvokeAsync(StateHasChanged);
	}
	private async Task OnPageChanged(int newPage)
	{
		_currentPage = newPage;

		var newUrl = $"/notification?page={_currentPage}&pageSize={_pageSize}";
		NavigationManager.NavigateTo(newUrl);

		await LoadData();
	}

	private (int, int) SetOrGetPageQueryParameters()
	{
		var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

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


			var newUrl = $"/notification?page={_currentPage}&pageSize={_pageSize}";

			NavigationManager.NavigateTo(newUrl);
			StateHasChanged();
		}
		return (page, size);
	}

	private async Task OnPageSizeChanged(int newCount)
	{
		Console.WriteLine(newCount);
		_pageSize = newCount;
		var newUrl = $"/notification?page={_currentPage}&pageSize={_pageSize}";
		NavigationManager.NavigateTo(newUrl);
		await LoadData();
	}
	private void ShowDetails(NotificationModel model)
		=> NotificationHelper.GoToNotificationDetails(model, NavigationManager);

	private async Task<string> FromUtc(DateTime dateUtc)
		=> (await TimeProvider.FromUtcToLocal(dateUtc)).ToString("HH:mm dd.MM.yyyy");
}
