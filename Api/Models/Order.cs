namespace Api.Models;

public class Order
{
	public int Id { get; set; }
	public string Description { get; set; }
	public List<Product> Products { get; set; }
	public decimal TotalPrice { get; set; }
}