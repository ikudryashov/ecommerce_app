using FluentValidation;

namespace ECommerceAppApi.Services.Categories.GetCategoryByName;

public class GetCategoryByNameQueryValidator : AbstractValidator<GetCategoryByNameQuery>
{
	public GetCategoryByNameQueryValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
	}
}