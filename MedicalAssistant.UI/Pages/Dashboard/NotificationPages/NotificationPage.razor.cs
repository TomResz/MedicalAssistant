﻿using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using System.Web;

namespace MedicalAssistant.UI.Pages.Dashboard.NotificationPages;

public partial class NotificationPage
{
	private List<NotificationModel> _notifications;


	private int _currentPage;
	private int _pageSize = 5;
	private int _pageCount = 0;
	private int _totalItemCount = 0;
	private readonly List<int> _pageSizes = [5, 10, 15, 25, 50, 100];
	private bool _loading = false;

	private Dictionary<DateTime, string> _formattedDates = [];

	[Inject] public NavigationManager NavigationManager { get; set; }
	[Inject] public INotificationService NotificationService { get; set; }
	[Inject] public ILocalTimeProvider TimeProvider { get; set; }
	protected override async Task OnInitializedAsync()
	{
		_loading = true;
		(_currentPage, _pageSize) = SetOrGetPageQueryParameters("notification",NavigationManager);
		await LoadData();
		_loading = false;
	}

	private async Task LoadData()
	{
		Response<PagedList<NotificationModel>> response = await NotificationService.GetPage(_currentPage, _pageSize);
		if (response.IsSuccess)
		{
			_notifications = response.Value!.Items;
			_pageCount = response.Value!.PageTotalCount;
			_totalItemCount = response.Value!.TotalCount;
		}
		foreach (var notification in _notifications)
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

	private (int, int) SetOrGetPageQueryParameters(string routePrefix,NavigationManager navigation)
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
		var newUrl = $"/notification?page={_currentPage}&pageSize={_pageSize}";
		NavigationManager.NavigateTo(newUrl);
		await LoadData();
	}

	private async Task<string> FromUtc(DateTime dateUtc)
		=> (await TimeProvider.FromUtcToLocal(dateUtc)).ToString("HH:mm dd.MM.yyyy");
}
