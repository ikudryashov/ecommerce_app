using FluentValidation;

namespace ECommerceAppApi.Services.Products.ListProductsInCategory;

public class ListProductsInCategoryQueryValidator : AbstractValidator<ListProductsInCategoryQuery>
{
	public ListProductsInCategoryQueryValidator()
	{
		RuleFor(x => x.CategoryName).NotEmpty();
	}
	
}