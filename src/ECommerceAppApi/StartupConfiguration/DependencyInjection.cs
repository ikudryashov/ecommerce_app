using System.Reflection;
using ECommerceAppApi.Domain.Exceptions;
using ECommerceAppApi.Infrastructure.Persistence;
using ECommerceAppApi.Middleware;
using ECommerceAppApi.Services.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ECommerceAppApi.StartupConfiguration;

public static class DependencyInjection
{
	public static IServiceCollection ConfigureServices(
		this IServiceCollection services, IConfiguration configuration)
	{
		//Database
		services.AddDbContext<Database>(options =>
		{
			options.UseNpgsql(configuration.GetSection("Database")["ConnectionString"]);
		});
		services.AddTransient<IFakeDataGenerator, FakeDataGenerator>();
		
		//MediatR
		services.AddMediatR(cfg => 
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
		
		//FluentValidation
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		//Exception handling
		services.AddTransient<GlobalExceptionHandlingMiddleware>();
		
		//Swagger
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();
		
		return services;
	}
}