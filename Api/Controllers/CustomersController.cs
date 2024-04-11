using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Api.Models;

namespace Api.Controllers;

 
// Really should have [Authorize] here but I am considering this out of scope
[ApiController]
[Route("api/v1/customers")]
public class CustomersController : ControllerBase
{
	private readonly IOrderRepository _orderRepository;

	public CustomersController(IOrderRepository orderRepository) 
	{
		_orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

	}

	[HttpGet]
	[Route("{id}/orders")]
	public async Task<IEnumerable<Order>> GetOrders(int id)
	{
		var orders = await _orderRepository.GetOrdersByCompany(id);

		return orders.Select(x => new Order
		{
			Id = x.OrderId,
			Description = x.Description,
			Products = x.Orderproducts.Select(y => new Product
			{
				Name = y.Product.Name,
				Quantity = y.Quantity,
				Price = y.Price ?? 0
			}).ToList(),
			TotalPrice = x.Orderproducts.Sum(y => (y.Price ?? 0) * y.Quantity)

		}).ToList();
	}

	// Stub methods below are examples of what other api method would go in this controller
	// If we found that this was getting bloated with endpoints like:
	// customers/{id}/payment-methods
	// customers/{id}/contact-details
	// customers/{id}/addresses
	// I would suggest moving them to individual controller that can handle the CRUD operations on that data
	[HttpGet]
	[Route("")]
	public void GetCustomers()
	{

	}


	[HttpGet]
	[Route("{id}")]
	public void GetCustomer()
	{

	}
}