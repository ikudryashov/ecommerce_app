namespace ECommerceAppApi.Services.Products;

public record ProductResult(
	Guid Id,
	Guid CategoryId,
	string Name,
	string CategoryName,
	string Description,
	decimal Price,
	string Color,
	int Quantity);