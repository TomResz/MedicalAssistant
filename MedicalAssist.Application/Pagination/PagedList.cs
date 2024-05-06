﻿namespace MedicalAssist.Application.Pagination;
public sealed class PagedList<T>
{
	public PagedList(List<T> items, int page, int pageSize, int totalCount)
	{
		Items = items;
		Page = page;
		PageSize = pageSize;
		TotalCount = totalCount;
	}
	public List<T> Items { get; set; }
	public int Page { get; set; }
	public int PageSize { get; set; }
	public int PageTotalCount => (TotalCount + PageSize - 1) / PageSize;
	public int TotalCount { get; set; }
	public bool HasNextPage => Page * PageSize < TotalCount;
	public bool HasPreviousPage => Page > 1;
}
