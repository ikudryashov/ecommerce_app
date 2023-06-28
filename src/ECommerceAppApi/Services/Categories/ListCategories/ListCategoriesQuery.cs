using MediatR;

namespace ECommerceAppApi.Services.Categories.ListCategories;

public record ListCategoriesQuery() : IRequest<List<CategoryResult>>;