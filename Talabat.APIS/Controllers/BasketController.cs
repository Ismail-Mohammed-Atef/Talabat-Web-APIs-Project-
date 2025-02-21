using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Dtos;
using Talabat.APIS.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Repository.Interfaces;

namespace Talabat.APIS.Controllers
{

	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository , IMapper mapper)
		{
			_basketRepository = basketRepository;
			_mapper = mapper;
		}


		[HttpGet]
		public async Task<ActionResult<CustomerBasket>> GetBasketAsync(string id)
		{
			var basket = await _basketRepository.GetBasket(id);
			if (basket == null) { return new CustomerBasket() { Id = id }; }

			return Ok(basket);
		}
		[HttpPost]
		public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket(CustomerBasketDto model)
		{
			var basket = _mapper.Map<CustomerBasket>(model);
			var CreatedOrUpdated = await _basketRepository.UpdateBasket(basket);
			if (CreatedOrUpdated == null) { return Ok(BadRequest(new ApiResponse(400))); }


			return Ok(CreatedOrUpdated);
		}

		[HttpDelete]
		public async void DeleteBasketAsync(string id)
		{
			await _basketRepository.DeleteBasket(id);
		}
	}
}
