using System.ComponentModel.DataAnnotations;

namespace ECommerceAppApi.Domain.Models;

public class Product
{
	[Required, MaxLength(36)]
	public Guid Id { get; init; }
	
	[Required, MaxLength(36)]
	public Guid CategoryId { get; init; }
	
	[Required, MaxLength(30)]
	public string Name { get; init; } = null!;
	
	[MaxLength(500)]
	public string Description { get; init; } = null!;
	
	[Required]
	public decimal Price { get; set; }
	
	[Required, MaxLength(20)]
	public string Color { get; init; } = null!;
	
	[Required]
	[Range(0, int.MaxValue)]
	public int Quantity { get; set; }
}