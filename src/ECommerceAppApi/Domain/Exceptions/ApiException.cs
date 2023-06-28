using System.Net;
using FluentValidation;
using FluentValidation.Results;

namespace ECommerceAppApi.Domain.Exceptions;

public class ApiException : Exception
{
	public string Type { get; }
	public string Title { get; }
	public string Detail { get;}
	public int StatusCode { get; }
	public List<ValidationFailure>? ValidationFailures { get; }

	public ApiException(
		string type,
		string title,
		string detail,
		HttpStatusCode statusCode,
		List<ValidationFailure>? failures = null)
	{
		Type = type;
		Title = title;
		Detail = detail;
		StatusCode = (int)statusCode;
		if (failures is not null)
		{
			ValidationFailures = failures;
		}
	}
}