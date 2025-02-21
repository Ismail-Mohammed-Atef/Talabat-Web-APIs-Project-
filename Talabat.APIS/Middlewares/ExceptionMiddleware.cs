using System.Text.Json;
using Talabat.APIS.Errors;

namespace Talabat.APIS.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger , IHostEnvironment env)
        {
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch(Exception e) 
			{
				_logger.LogError(e, e.Message);

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;

				var response = _env.IsDevelopment() ? new ApiException(500, e.Message, e.StackTrace.ToString()) : new ApiException(500);

				var result = JsonSerializer.Serialize(response);

				await context.Response.WriteAsync(result);
			}
		}
    }
}
