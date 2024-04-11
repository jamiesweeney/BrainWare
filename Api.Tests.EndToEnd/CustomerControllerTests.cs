using System.Net;
using System.Text.Json;
using Data;
using Data.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Order = Api.Models.Order;

namespace Api.Tests.EndToEnd
{
	// Note: You'll need docker to make this work
	// This test uses a web application factory to run the api service, it uses the same docker database as the integration tests.
	// We stop the service from using our local DB by overwriting the dependency injection.
	
	public class CustomerControllerTests
	{
		private readonly MsSqlContainer _container;
		private readonly OrderContext _context;
		private readonly HttpClient _client;

		public CustomerControllerTests()
		{
			_container = new MsSqlBuilder().Build();
			_container.StartAsync().Wait();

			var optionsBuilder =
				new DbContextOptionsBuilder<OrderContext>().UseSqlServer(_container.GetConnectionString());
			_context = new OrderContext(optionsBuilder.Options);
			_context.Database.Migrate();

			var factory = new WebApplicationFactory<Api.Program>().WithWebHostBuilder(builder =>
			{
				builder.ConfigureServices(services =>
				{

					var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<OrderContext>));
					if (descriptor != null)
						services.Remove(descriptor);

					services.AddSqlServer<OrderContext>(
						_container.GetConnectionString());
				});

				builder.UseEnvironment("Development");
			});
			_client = factory.CreateClient();
		}

		[Fact]
		public async Task GetOrdersForCustomer_NoOrders()
		{
			var response = await _client.GetAsync($"api/v1/customers/1/orders");

			response.StatusCode.Should().Be(HttpStatusCode.OK);
			var content = await JsonSerializer.DeserializeAsync<List<Order>>(await response.Content.ReadAsStreamAsync());

			content.Should().BeEmpty();
		}

		[Fact]
		public async Task GetOrdersForCustomer_HasOrders()
		{
			var order = new Data.Models.Order
			{
				Description = "TestOrder",
				CompanyId = 1,
				Company = new Company
				{
					CompanyId = 1,
					Name = "TestCompany",
				},
				Orderproducts = new List<Orderproduct>
				{
					new Orderproduct
					{
						ProductId = 1,
						Price = 1,
						Quantity = 2,
						Product = new Product
						{
							ProductId = 1,
							Name = "TestProduct",
							Price = 2,
						}
					}
				}
			};

			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			var response = await _client.GetAsync($"api/v1/customers/1/orders");

			response.StatusCode.Should().Be(HttpStatusCode.OK);
			var stringContent = await response.Content.ReadAsStringAsync();
			var content = JsonSerializer.Deserialize<IEnumerable<Order>>(stringContent, new JsonSerializerOptions(){PropertyNameCaseInsensitive = true});

			content.Should().HaveCount(1);
			var actualOrder = content.Single();

			actualOrder.Id.Should().Be(1);
			actualOrder.Description.Should().Be(order.Description);
			actualOrder.TotalPrice.Should().Be(2);
			actualOrder.Products.Should().HaveCount(1);
			var actualProduct = actualOrder.Products.Single();
			actualProduct.Quantity.Should().Be(order.Orderproducts.First().Quantity);
			actualProduct.Name.Should().Be(order.Orderproducts.First().Product.Name);
			actualProduct.Price.Should().Be(order.Orderproducts.First().Price);
		}

		public async Task Dispose()
		{
			_client.Dispose();
			await _container.StopAsync();
		}
	}
}