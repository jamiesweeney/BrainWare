using Data;
using Data.Repositories;

namespace Api;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		builder.Services.AddControllers();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddSqlServer<OrderContext>(
			"Server=localhost\\SQLEXPRESS;Database=BrainWAre;Integrated Security=SSPI;Trusted_Connection=True;TrustServerCertificate=True");
		builder.Services.AddScoped<IOrderRepository, OrderRepository>();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		// app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();

	}
}