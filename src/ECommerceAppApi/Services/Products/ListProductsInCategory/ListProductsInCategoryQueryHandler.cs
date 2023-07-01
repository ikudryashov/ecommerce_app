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
		var category = await _context.Categories
			.SingleOrDefaultAsync(c => 
				c.Name.ToLower() == request.CategoryName!.ToLower(), cancellationToken);
		
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

		productsQuery = 
			request.SortOrder?.ToLower() == "desc" ? 
			productsQuery.OrderByDescending(keySelector) : 
			productsQuery.OrderBy(keySelector);


		if (request.OnlyInStock is not null && (bool)request.OnlyInStock)
		{
			productsQuery = productsQuery.Where(p => p.Quantity > 0);
		}
		
		productsQuery = productsQuery.Where(p => p.Quantity >= 0);

		var productResultsQuery = productsQuery
			.Select(p => new ProductResult(
			p.Id,
			p.CategoryId,
			p.Name,
			category.Name,
			p.Description,
			p.Price,
			p.Color,
			p.Quantity));

		var products = await PagedList<ProductResult>
			.CreateAsync(productResultsQuery, request.Page, request.PageSize);

		return products;
	}
}