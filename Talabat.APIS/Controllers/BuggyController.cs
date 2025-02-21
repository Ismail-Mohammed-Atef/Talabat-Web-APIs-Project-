using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIS.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIS.Controllers
{
	
	public class BuggyController : BaseApiController
	{
		private readonly StoreDbContext _context;

		public BuggyController(StoreDbContext context)
		{
			_context = context;
		}

		[HttpGet("notfound")]
		public ActionResult GetNotFoundRequest()
		{
			var product = _context.Products.Find(1000);
			if (product == null)
			{
				return NotFound(new ApiResponse(404));
			}
			return Ok(product);
		}

		[HttpGet("servererror")]
		public ActionResult GetServerError()
		{
			var product = _context.Products.Find(1000);
			var result = product.ToString();

			return Ok(result);
		}

		[HttpGet("badrequest")]
		public ActionResult GetBadRequest()
		{
			return BadRequest(new ApiResponse(400));
		}

		[HttpGet("badrequest/{id}")]

		public ActionResult GetBadRequest(int? id)
		{
			return Ok();
		}

		[HttpGet("unauthorized")]
		public ActionResult GetUnAuthorizedError()
		{
			return Unauthorized(new ApiResponse(401));
		}
	}
}
