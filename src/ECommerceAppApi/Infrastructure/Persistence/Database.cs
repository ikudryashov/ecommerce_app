using ECommerceAppApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAppApi.Infrastructure.Persistence;

public class Database : DbContext
{
	public Database(DbContextOptions<Database> options) : base(options)
	{ }

	public DbSet<Product> Products { get; set; } = null!;
	public DbSet<Category> Categories { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Category>()
			.HasIndex(c => new { c.Name })
			.IsUnique();
	}
}