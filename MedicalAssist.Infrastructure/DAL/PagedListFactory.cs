﻿using MedicalAssist.Application.Pagination;
using Microsoft.EntityFrameworkCore;

namespace MedicalAssist.Infrastructure.DAL;
public class PagedListFactory<T>
{
	public static async Task<PagedList<T>> CreateByQueryAsync(IQueryable<T> query, int page, int pageSize)
	{
		var totalCount = await query.CountAsync();
		var items = await query
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();
		return new PagedList<T>(items, page, pageSize, totalCount);
	}
}