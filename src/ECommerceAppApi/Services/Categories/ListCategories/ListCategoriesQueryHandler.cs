using ECommerceAppApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAppApi.Services.Categories.ListCategories;

public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, List<CategoryResult>>
{
	private readonly Database _context;

	public ListCategoriesQueryHandler(Database context)
	{
		_context = context;
	}

	public async Task<List<CategoryResult>> Handle(ListCategoriesQuery query, CancellationToken cancellationToken)
	{
		 return  await _context.Categories
			 .Select(c => 
				 new CategoryResult(c.Id, c.Name, c.Description)).ToListAsync(cancellationToken);
	}
}