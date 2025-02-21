using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Talabat.APIS.Errors;
using Talabat.APIS.Extensions;
using Talabat.APIS.Helper;
using Talabat.APIS.Middlewares;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Repository.Interfaces;
using Talabat.Core.Services.Interfaces;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Repositories;
using Talabat.Services;

namespace Talabat.APIS
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<StoreDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
			{
				var connection = builder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			builder.Services.AddDbContext<AppIdentityDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});
			builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

			builder.Services.AddScoped<IBasketRepository,BasketRepository>();
			builder.Services.AddScoped<ITokenService,TokenService>();
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = builder.Configuration["JWT:Issuer"],
					ValidateAudience = true,
					ValidAudience = builder.Configuration["JWT:Audience"],
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
				};
			});

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("MyPolicy", config =>
				{
					config.AllowAnyHeader();
					config.AllowAnyMethod();
					config.WithOrigins(builder.Configuration["FrontEndBaseUrl"]);
				});
			});

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IOrderService, OrderService>();
			builder.Services.AddScoped<IpaymentService, PaymentService>();
			builder.Services.AddApplicationService();


			var app = builder.Build();

			var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var context = services.GetRequiredService<StoreDbContext>();

			var identityContext = services.GetRequiredService<AppIdentityDbContext>();

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await context.Database.MigrateAsync();
				await identityContext.Database.MigrateAsync();

				var usermanager = services.GetRequiredService<UserManager<AppUser>>();
					
				await AppIdentityDataSeeding.AddDataSeedAsync(usermanager);

				await StoreDbContextDataSeed.SeedAsync(context);

			}catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();

				logger.LogError(ex, "Coudlnt apply migrations");

			}

			app.UseMiddleware<ExceptionMiddleware>();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseStatusCodePagesWithRedirects("/errors/{0}");

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCors("MyPolicy");
			app.UseAuthorization();
			app.UseAuthentication();

			app.MapControllers();

			app.Run();
		}
	}
}
