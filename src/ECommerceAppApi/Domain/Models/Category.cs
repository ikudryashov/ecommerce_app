using System.ComponentModel.DataAnnotations;

namespace ECommerceAppApi.Domain.Models;

public class Category
{
	[Required, MaxLength(36)]
	public Guid Id { get; init; }
	
	[Required, MaxLength(30)]
	public string Name { get; init; } = null!;
	
	[MaxLength(500)]
	public string Description { get; init; } = null!;

	public virtual List<Product>? Products { get; init; } = new ();
}