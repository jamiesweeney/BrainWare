using Data.Models;

namespace Data.Repositories;

public interface IOrderRepository
{
	Task<List<Order>> GetOrdersByCompany(int company);
}