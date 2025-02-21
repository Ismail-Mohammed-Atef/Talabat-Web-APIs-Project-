using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Talabat.APIS.Dtos;
using Talabat.APIS.Errors;
using Talabat.Core.Models.Orders;
using Talabat.Core.Repository.Interfaces;
using Talabat.Core.Services.Interfaces;

namespace Talabat.APIS.Controllers
{
	
	public class OrdersController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public OrdersController(IOrderService orderService , IMapper mapper ,IUnitOfWork unitOfWork)
        {
			_orderService = orderService;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		[ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
		[HttpPost]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderToDto model)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);

			var address = _mapper.Map<AddressDto,Address>(model.ShippingAddress);
			var order = await _orderService.CreateOrderAsync(email, model.BasketId, model.DeliveryMethodId, address);

			if(order == null) { return BadRequest(new ApiResponse(400, "Couldnt make your order")); }

			var result =   _mapper.Map<OrderToReturnDto>(order);

			return Ok(result);

		}

		[ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrders()
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var orders = await _orderService.GetOrdersOfSpecificUserAsync(buyerEmail);
			if(orders == null) { return NotFound(new ApiResponse(404, "Order not Found")); }



			return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
		}

		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

		public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
		{
			var orderEmail = User.FindFirstValue(ClaimTypes.Email);
			var order = await _orderService.GetOrderForSpecificUserAsync(orderEmail,id);
			if(order == null) { return NotFound(new ApiResponse(404, "no order with that id for you")); }


			return Ok(_mapper.Map<OrderToReturnDto>(order));
		}

		[ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("deliveryMethods")]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

			return Ok(deliveryMethods);
		}
	}
}
