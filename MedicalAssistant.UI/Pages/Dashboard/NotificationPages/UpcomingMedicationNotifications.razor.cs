using MedicalAssistant.UI.Models.MedicationNotification;
using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using MedicalAssistant.UI.Shared.Services.Time;
using Microsoft.AspNetCore.Components;
using System.Web;

namespace MedicalAssistant.UI.Pages.Dashboard.NotificationPages;

public partial class UpcomingMedicationNotifications
{
	private List<MedicationNotificationPageContentDto> _notifications = [];

	private int _currentPage;
	private int _pageSize = 5;
	private int _pageCount = 0;
	private int _totalItemCount = 0;
	private readonly static List<int> _pageSizes = [5, 10, 15, 25, 50, 100];
	private bool _loading = false;

	private const string RoutePrefix = "notification/medication";

	[Inject] public NavigationManager NavigationManager { get; set; }
	[Inject] public IMedicationNotificationService NotificationService { get; set; }
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
		var currentDate = await TimeProvider.CurrentDate();
		var offset = await TimeProvider.TimeZoneOffset();

		var response = await NotificationService.GetPagedList(_currentPage, _pageSize,currentDate,offset);
		if (response.IsSuccess)
		{
			_notifications = response.Value!.Items;
			_pageCount = response.Value!.PageTotalCount;
			_totalItemCount = response.Value!.TotalCount;
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
		_pageSize = newCount;
		var newUrl = $"/{RoutePrefix}?page={_currentPage}&pageSize={_pageSize}";
		NavigationManager.NavigateTo(newUrl);
		await LoadData();
	}

	private async Task Delete(Guid id)
	{

	}
}
