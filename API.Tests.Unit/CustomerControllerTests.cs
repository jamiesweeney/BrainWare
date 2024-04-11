using Api.Controllers;
using Data.Models;
using Data.Repositories;
using FluentAssertions;
using Moq;

namespace API.Tests.Unit
{
	public class CustomerControllerTests
	{
		private readonly CustomersController _controller;

		private static readonly List<Order> TestOrders = new()
		{
			new()
			{
				OrderId = 1,
				Description = "Test Order 1",
				CompanyId = 1,
				Company = new Company() { Name = "Test Company" },
				Orderproducts = new List<Orderproduct>
				{
					new()
					{
						Product = new Product
						{
							ProductId = 1,
							Name = "Test Product 1",
							Price = 1,
						},
						Price = 2,
						Quantity = 10
					},
					new()
					{
						Product = new Product
						{
							ProductId = 2,
							Name = "Test Product 2",
							Price = 2,
						},
						Price = 3.5M,
						Quantity = 100
					}
				}
			}
		};

		public CustomerControllerTests()
		{
			var repositoryMock = new Mock<IOrderRepository>();
			repositoryMock.Setup(x => x.GetOrdersByCompany(1)).ReturnsAsync(new List<Order>());
			repositoryMock.Setup(x => x.GetOrdersByCompany(2)).ReturnsAsync(TestOrders);

			_controller = new CustomersController(repositoryMock.Object);
		}


		[Fact]
		public async Task GetOrders_WhenCalledWithCustomerIdWithNoOrders_ReturnsEmptyList()
		{
			var result = await _controller.GetOrders(1);

			result.Should().BeEmpty();
		}

		[Fact]
		public async Task GetOrders_WhenCalledWithCustomerIdWithOrders_ReturnsOrder()
		{
			var expectedResult = new Api.Models.Order
			{
				Id = 1,
				Description = "Test Order 1",
				Products = new List<Api.Models.Product>
				{
					new()
					{
						Name = "Test Product 1",
						Quantity = 10,
						Price = 2
					},
					new()
					{
						Name = "Test Product 2",
						Quantity = 100,
						Price = 3.5m
					},
				},
				TotalPrice = 370
			};

			var result = (await _controller.GetOrders(2)).ToList();

			result.Should().HaveCount(1);
			var order = result.Single();
			order.Should().BeEquivalentTo(expectedResult);
		}
	}
}