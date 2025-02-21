using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;

namespace Talabat.APIS.Controllers
{
	[Route("errors/{statusCode}")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorsController : ControllerBase
	{
		public IActionResult errors(int statusCode)
		{
			return NotFound(new ApiResponse(statusCode));
		}
	}
}
