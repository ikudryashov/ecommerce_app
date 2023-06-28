using System.Net;
using ECommerceAppApi.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace ECommerceAppApi.Services.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IValidator<TRequest>? _validator;

	public ValidationBehavior(IValidator<TRequest>? validator = null)
	{
		_validator = validator;
	}

	public async Task<TResponse> Handle(
		TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (_validator is null)
		{
			return await next();
		}

		var validationResult = await _validator.ValidateAsync(request, cancellationToken);

		if (validationResult.IsValid)
		{
			return await next();
		}

		throw new ApiException(
			"https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
			"One or more validation errors occured",
			"See additional information for details",
			HttpStatusCode.BadRequest,
			validationResult.Errors);
	}
}