using MediatR;

namespace ECommerceAppApi.Services.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductResult>;