using Bogus;
using ECommerceAppApi.Domain.Models;
using ECommerceAppApi.StartupConfiguration;

namespace ECommerceAppApi.Infrastructure.Persistence;

public class FakeDataGenerator : IFakeDataGenerator
{
	private readonly Database _dbContext;

	public FakeDataGenerator(Database dbContext)
	{
		_dbContext = dbContext;
	}
	public async Task PopulateWithFakeDataAsync()
	{
		Randomizer.Seed = new Random(8675309);
		string[] colors = { "Black", "White", "Red", "Blue", "Yellow" };
		
		var testCategories = new Faker<Category>()
			.RuleFor(x => x.Id, f => f.Random.Guid())
			.RuleFor(x => x.Name, f => f.PickRandom(f.Commerce.Categories(5)))
			.RuleFor(x => x.Description, f => f.Lorem.Sentences(1))
			.Generate(5); 
		
		var testProducts = new Faker<Product>()
			.RuleFor(x => x.Id, f => f.Random.Guid())
			.RuleFor(x => x.CategoryId, f => f.PickRandom(testCategories).Id)
			.RuleFor(x => x.Name, f => f.Commerce.ProductName())
			.RuleFor(x => x.Description, f => f.Lorem.Sentences(1))
			.RuleFor(x => x.Price, f => f.Random.Decimal(1, 1000))
			.RuleFor(x => x.Color, f => f.PickRandom(colors))
			.FinishWith((f, x) =>
			{
				var category = testCategories.FirstOrDefault(c => c.Id == x.CategoryId);
				category!.Products!.Add(x);
			})
			.Generate(10000);

		await _dbContext.Categories.AddRangeAsync(testCategories);
		await _dbContext.Products.AddRangeAsync(testProducts);
		await _dbContext.SaveChangesAsync();
	}
}