using System.Linq.Expressions;
using System.Net;
using ECommerceAppApi.Domain.Exceptions;
using ECommerceAppApi.Domain.Models;
using ECommerceAppApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAppApi.Services.Products.ListProductsInCategory;

public class ListProductsInCategoryQueryHandler : IRequestHandler<ListProductsInCategoryQuery, PagedList<ProductResult>>
{
	private readonly Database _context;

	public ListProductsInCategoryQueryHandler(Database context)
	{
		_context = context;
	}
	
	public async Task<PagedList<ProductResult>> Handle(ListProductsInCategoryQuery request, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(request.CategoryName))
		{
			throw new ApiException(
				"https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
				"Bad request",
				"No category name provided",
				HttpStatusCode.BadRequest);
		}
		
		var category = await _context.Categories
			.SingleOrDefaultAsync(c => c.Name == request.CategoryName, cancellationToken);
		
		if (category is null)
		{
			throw new ApiException(
				"https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
				"Not found",
				"Category with provided name does not exist",
				HttpStatusCode.NotFound);
		}
		
		var productsQuery = _context.Products.AsQueryable();
		
		productsQuery = productsQuery.Where(p => p.CategoryId == category.Id);

		if (!string.IsNullOrWhiteSpace(request.SearchTerm))
		{
			productsQuery = productsQuery
				.Where(p => p.Name.Contains(request.SearchTerm));
		}

		Expression<Func<Product, object>> keySelector = request.SortColumn?.ToLower() switch
		{
			"name" => product => product.Name,
			"price" => product => product.Price,
			"color" => product => product.Color,
			_ => product => product.Id
		};

		if (request.SortOrder?.ToLower() == "desc")
		{
			productsQuery = productsQuery.OrderByDescending(keySelector);
		}
		else
		{
			productsQuery = productsQuery.OrderBy(keySelector);
		}

		//arbitrary default values in case provided pagination values are invalid
		int page = 1;
		int pageSize = 10;
		
		if (request.Page is not null && request.Page >= 1)
		{
			page = (int)request.Page;
		}

		if (request.PageSize is not null && request.PageSize >= 1)
		{
			pageSize = (int)request.PageSize;
		}

		var productResultsQuery = productsQuery
			.Select(p => new ProductResult(
			p.Id,
			p.CategoryId,
			p.Name,
			category.Name,
			p.Description,
			p.Price,
			p.Color));

		var products = await PagedList<ProductResult>
			.CreateAsync(productResultsQuery, page, pageSize);

		return products;
	}
}