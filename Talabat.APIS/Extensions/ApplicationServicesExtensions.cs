using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Talabat.APIS.Errors;
using Talabat.APIS.Helper;
using Talabat.Core.Repository.Interfaces;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;

namespace Talabat.APIS.Extensions
{
	public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationService(this IServiceCollection services)
		{
			
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			services.AddAutoMapper(typeof(MappingProfile));
			//builder.Services.AddAutoMapper(m=>m.AddProfile(new MappingProfile()));

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (ActionContext) =>
				{
					var errors = ActionContext.ModelState.Where(m => m.Value.Errors.Count() > 0).SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToArray();

					var validationErrorResponse = new ApiValidationErrorResponse()
					{
						Errors = errors
					};
					return new BadRequestObjectResult(validationErrorResponse);
				};
			});

			

			return services;
		}
	}
}
