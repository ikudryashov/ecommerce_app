using MediatR;

namespace ECommerceAppApi.Services.Categories.GetCategoryByName;

public record GetCategoryByNameQuery(string Name) : IRequest<CategoryResult>;