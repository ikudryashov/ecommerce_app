using System.Net;
using ECommerceAppApi.Domain.Exceptions;
using ECommerceAppApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAppApi.Services.Categories.GetCategoryByName;

public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, CategoryResult>
{
	private readonly Database _context;

	public GetCategoryByNameQueryHandler(Database context)
	{
		_context = context;
	}

	public async Task<CategoryResult> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
	{
		var category = await _context.Categories.SingleOrDefaultAsync(
			c => c.Name == request.Name, cancellationToken: cancellationToken);

		if (category is null)
		{
			throw new ApiException(
				"https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
				"Category not found",
				"Try changing the category name", 
				HttpStatusCode.NotFound);
		}

		return new CategoryResult(category.Id, category.Name, category.Description);
	}
}