using Microsoft.EntityFrameworkCore;

namespace ECommerceAppApi.Services.Products.ListProductsInCategory;

public class PagedList<T>
{
	private PagedList(List<T> items, int page, int pageSize, int totalCount)
	{
		Items = items;
		Page = page;
		PageSize = pageSize;
		TotalCount = totalCount;
	}
	
	public List<T> Items { get; }
	public int Page { get; }
	public int PageSize { get; }
	public int TotalCount { get; }

	public bool NextPageExists => Page * PageSize < TotalCount;
	public bool PrevPageExists => Page > 1;

	public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int? requestPage, int? requestPageSize)
	{
		//arbitrary default values in case provided pagination values are invalid
		int page = 1;
		int pageSize = 10;
		
		if (requestPage is not null && requestPage >= 1)
		{
			page = (int)requestPage;
		}

		if (requestPageSize is not null && requestPageSize >= 1)
		{
			pageSize = (int)requestPageSize;
		}
		
		int totalCount = await query.CountAsync();
		var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
		return new(items, page, pageSize, totalCount);
	}
}