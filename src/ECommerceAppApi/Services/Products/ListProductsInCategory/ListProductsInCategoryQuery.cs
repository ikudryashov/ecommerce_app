using MediatR;

namespace ECommerceAppApi.Services.Products.ListProductsInCategory;

public record ListProductsInCategoryQuery(
	string? CategoryName,
	string? SearchTerm,
	string? SortColumn,
	string? SortOrder,
	bool? OnlyInStock,
	int? Page,
	int? PageSize) : IRequest<PagedList<ProductResult>>;