using ECommerceAppApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAppApi.StartupConfiguration;

public static class WebApplicationConfiguration
{
	public static WebApplication ConfigureSwagger(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		return app;
	}
	
	public static async Task<WebApplication> InitDatabase(this WebApplication app)
	{
		await using var scope = app.Services.CreateAsyncScope();
		var services = scope.ServiceProvider;
		var context = services.GetRequiredService<Database>();
		var fakeDataGenerator = services.GetRequiredService<IFakeDataGenerator>();
	
		await context.Database.EnsureCreatedAsync();

		if (await context.Products.AnyAsync() == false
		    && await context.Categories.AnyAsync() == false)
		{
			await fakeDataGenerator.PopulateWithFakeDataAsync();
		}

		return app;
	}
}