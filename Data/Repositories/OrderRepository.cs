using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly OrderContext _orderContext;

		public OrderRepository(OrderContext orderContext)
		{
			_orderContext = orderContext ?? throw new ArgumentNullException(nameof(orderContext));
		}

		public Task<List<Order>> GetOrdersByCompany(int company)
		{
			return _orderContext.Orders
				.Include(x => x.Company)
				.Include(x => x.Orderproducts)
				.ThenInclude(x => x.Product)
				.Where(x => x.CompanyId == company)
				.ToListAsync();
		}
	}
}
