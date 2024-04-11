using Data.Models;
using Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace Data.Tests.Integration
{

	// Note: You'll need docker to make this work
	// This integration test will spin up a MSSQL database within a container, apply the migrations to the database, populate the database and then run some tests.
	// I much prefer this way of testing database connections rather than connecting to a test / prod database since it's a lot easier for a dev to run completely locally
	// and is less prone to breaking. 
	// This is what I would refer to as a "shallow integration test" as opposed to a "broad integration test", a broad one might include testing the whole request flow from controller to database as one unit,
	// in my opinion that scale of test should be reserved for an end to end test where you actually hit the endpoint on a deployed instance of the service.
	public class OrderRepositoryTests
	{
		private readonly MsSqlContainer _container;
		private readonly OrderContext _context;
		private readonly OrderRepository _repository;
		
		
		private List<Product> TestProducts = new()
		{
			new ()
			{
				ProductId = 1,
				Name = "Test Product 1",
				Price = 1,
			},
			new ()
			{
				ProductId = 2,
				Name = "Test Product 2",
				Price = 2,
			},
			new()
			{
				ProductId = 3,
				Name = "Test Product 3",
				Price = 3,
			}
		};

		private List<Company> TestCompanies = new List<Company>
		{
			new()
			{
				CompanyId = 1,
				Name = "Test Company 1",
			},
			new()
			{
				CompanyId = 2,
				Name = "Test Company 2",
			},
		};

		public OrderRepositoryTests()
		{
			_container = new MsSqlBuilder().Build();
			_container.StartAsync().Wait();

			var optionsBuilder =
				new DbContextOptionsBuilder<OrderContext>().UseSqlServer(_container.GetConnectionString());
			_context = new OrderContext(optionsBuilder.Options);
			_context.Database.Migrate();
			_context.Products.AddRange(TestProducts);
			_context.Companies.AddRange(TestCompanies);
			_context.SaveChanges();

			_repository = new OrderRepository(_context);
		}
		
		
		[Fact]
		public async Task GetOrdersByCompany_WhenCalledForCompanyWithNoOrders_ReturnsEmptyList()
		{
			var orders = await _repository.GetOrdersByCompany(1);

			orders.Should().BeEmpty();
		}

		[Fact]
		public async Task GetOrdersByCompany_WhenCalledForCompanyWithOrders_ReturnsOrders()
		{
			var orders = new List<Order>
			{
				new()
				{
					Description = "Test Order 1",
					CompanyId = 2,
					Orderproducts = new List<Orderproduct>
					{
						new()
						{
							ProductId = 1,
							Price = 1,
							Quantity = 2,
						},
						new()
						{
							ProductId = 2,
							Price = 2,
							Quantity = 4,
						}
					}
				}
			};
			_context.Orders.AddRange(orders);
			await _context.SaveChangesAsync();

			var actualOrders = await _repository.GetOrdersByCompany(2);

			actualOrders.Should().BeEquivalentTo(orders);
		}

		public async Task Dispose()
		{
			await _container.StopAsync();
		}
	}
}