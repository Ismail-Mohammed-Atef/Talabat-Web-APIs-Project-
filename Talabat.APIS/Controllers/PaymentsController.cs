using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Talabat.APIS.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Repository.Interfaces;
using Stripe;
using Talabat.Core.Services.Interfaces;

namespace Talabat.APIS.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class PaymentsController : BaseApiController
	{
		private readonly IpaymentService _paymentService;
		const string endpointSecret = "whsec_8297dbe241c95fb71fff58a3c22e0f9def30b00229d21d1279604bcfc475e56a";


		public PaymentsController(IpaymentService paymentService)
		{
			_paymentService = paymentService;
		}

		[ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentService.CreateOrUpdatePaymentMethod(basketId);
			if (basket == null) { return BadRequest(new ApiResponse(400, "Couldnt Make you payment intent")); }
			return Ok(basket);

		}
		[AllowAnonymous]
		[HttpPost("webhook")]
		public async Task<IActionResult> StripeWebHook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			try
			{
				var stripeEvent = EventUtility.ConstructEvent(json,
					Request.Headers["Stripe-Signature"], endpointSecret);
				var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
				// Handle the event
				if (stripeEvent.Type == Events.PaymentIntentSucceeded)
				{
					await _paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id, true);
				}
				else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
				{
					await _paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id, false);

				}
				// ... handle other event types
				else
				{
					Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
				}

				return Ok();
			}
			catch (StripeException e)
			{
				return BadRequest();
			}
		}
	}
}
