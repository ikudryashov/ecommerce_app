using System.Net;
using ECommerceAppApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAppApi.Middleware;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
	private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

	public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
	{
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		//API-specific exception
		catch (ApiException ex)
		{
			_logger.LogError("Request failure {@Path} {@Error} {@DateTimeUtc}",
				context.Request.Path.ToString(),
				ex,
				DateTime.UtcNow);

			context.Response.StatusCode = ex.StatusCode;
			context.Response.ContentType = "application/problem+json";
			
			var problemDetails = new ProblemDetails
			{
				Status = ex.StatusCode,
				Type = ex.Type,
				Title = ex.Title,
				Detail = ex.Detail,
				Instance = context.Request.Path.ToString()
			};

			if (ex.ValidationFailures is not null)
			{
				problemDetails.Extensions.Add("validation errors", new Dictionary<string, List<string>>());
				var validationErrors = (Dictionary<string, List<string>>)problemDetails.Extensions["validation errors"]!;
				
				foreach (var failure in ex.ValidationFailures)
				{
					if (validationErrors.TryGetValue(failure.PropertyName, out var error))
					{
						error.Add(failure.ErrorMessage);
					}
					else
					{
						validationErrors.Add(failure.PropertyName, new List<string>() { failure.ErrorMessage });
					}
				}
			}

			await context.Response.WriteAsJsonAsync(problemDetails);
		}
		//unexpected internal exception
		catch (Exception ex)
		{
			_logger.LogError("Request failure {@Path} {@Error} {@DateTimeUtc}",
				context.Request.Path.ToString(),
				ex,
				DateTime.UtcNow);

			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			context.Response.ContentType = "application/problem+json";

			var problemDetails = new ProblemDetails
			{
				Status = (int)HttpStatusCode.InternalServerError,
				Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
				Title = "Internal server error",
				Detail = "Try again later",
				Instance = context.Request.Path.ToString()
			};

			await context.Response.WriteAsJsonAsync(problemDetails);
		}
	}
}