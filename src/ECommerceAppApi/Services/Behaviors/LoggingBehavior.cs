using MediatR;

namespace ECommerceAppApi.Services.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger<TRequest>? _logger;

	public LoggingBehavior(ILogger<TRequest>? logger = null)
	{
		_logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (_logger is null) return await next();
		
		_logger.LogInformation("Started request execution: {@Request} {@DateTimeUtc}",
			typeof(TRequest),
			DateTime.UtcNow);

		var res = await next();
		
		_logger.LogInformation("Finished request execution: {@Request} {@DateTimeUtc}",
			typeof(TRequest),
			DateTime.UtcNow);

		return res;
	}
}