using FluentValidation;

namespace ECommerceAppApi.Services.Products.GetProductById;

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
	public GetProductByIdQueryValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
	}
}