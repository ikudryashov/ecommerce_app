using ECommerceAppApi.Middleware;
using ECommerceAppApi.Services.Categories.GetCategoryByName;
using ECommerceAppApi.Services.Categories.ListCategories;
using ECommerceAppApi.Services.Products.GetProductById;
using ECommerceAppApi.Services.Products.ListProductsInCategory;
using ECommerceAppApi.StartupConfiguration;
using MediatR;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

await app.InitDatabase();
app.ConfigureSwagger();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();


app.MapGet("/api/categories/",
	async (IMediator mediator) => await mediator.Send(new ListCategoriesQuery()));

app.MapGet("/api/categories/{name}",
	async (IMediator mediator, string name) =>
	{
		var result = await mediator.Send(new GetCategoryByNameQuery(Name: name));
		return result;
	});

app.MapGet("api/{category}/products",
	async (
		IMediator mediator,
		string category,
		string? searchTerm,
		string? sortColumn,
		string? sortOrder,
		bool? onlyInStock,
		int? page,
		int? pageSize) => await mediator
		.Send(new ListProductsInCategoryQuery(
			category,
			searchTerm, 
			sortColumn, 
			sortOrder,
			onlyInStock,
			page,
			pageSize)));

app.MapGet("api/products/{id}",
 	async (IMediator mediator, Guid id) => await mediator.Send(new GetProductByIdQuery(id)));

app.UseHttpsRedirection();
app.Run();