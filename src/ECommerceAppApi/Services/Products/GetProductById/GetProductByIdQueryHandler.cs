using System.Net;
using ECommerceAppApi.Domain.Exceptions;
using ECommerceAppApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAppApi.Services.Products.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResult>
{
	private readonly Database _context;

	public GetProductByIdQueryHandler(Database context)
	{
		_context = context;
	}

	public async Task<ProductResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
	{
		var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == request.Id);
		if (product is null)
		{
			throw new ApiException(
				"https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
				"Not found",
				"Product with provided ID does not exist",
				HttpStatusCode.NotFound);
		}

		var category = await _context.Categories.SingleAsync(c => c.Id == product.CategoryId);
		
		return new ProductResult(
			product.Id, 
			category.Id,
			product.Name,
			category.Name,
			product.Description,
			product.Price,
			product.Color);
	}
}