using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Models.Orders;
using product = Talabat.Core.Entites.Product;
using Talabat.Core.Repository.Interfaces;
using Talabat.Repository.Repositories;
using Talabat.Core.Services.Interfaces;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Services
{
	public class PaymentService : IpaymentService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;

		public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
		{
			_basketRepository = basketRepository;
			_unitOfWork = unitOfWork;
			_configuration = configuration;
		}
		public async Task<CustomerBasket?> CreateOrUpdatePaymentMethod(string basketId)
		{
			var basket = await _basketRepository.GetBasket(basketId);
			if (basket == null) { return null; }
			if (basket.Items.Count() > 0)
			{
				foreach (var item in basket.Items)
				{
					var product = await _unitOfWork.Repository<product>().GetAsync(item.Id);
					if (item.Price != product.Price)
					{
						item.Price = product.Price;
					}
				}
			}
			var shippingPrice = 0m;
			var subTotal = basket.Items.Sum(i => (i.Price * i.Quantity));
			if (basket.DeliveryMethodId.HasValue)
			{
				var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
			}


			StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
			var service = new PaymentIntentService();
			if (string.IsNullOrEmpty(basket.PaymentIntentId))
			{
				var options = new PaymentIntentCreateOptions()
				{
					Amount = (long)(subTotal * 100 + shippingPrice * 100),
					Currency = "usd",
					PaymentMethodTypes = new List<string>() { "card" }
				};
				var paymentIntent = await service.CreateAsync(options);
				basket.PaymentIntentId = paymentIntent.Id;
				basket.ClientSecret = paymentIntent.ClientSecret;
			}
			else
			{
				var options = new PaymentIntentUpdateOptions()
				{
					Amount = (long)(subTotal * 100 + shippingPrice * 100),
				};
				var paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,options);
				basket.ClientSecret = paymentIntent.ClientSecret;
			}

			await _basketRepository.UpdateBasket(basket);

			return basket;
		}

		public async Task<Order> UpdatePaymentIntentToSucceedOrFailed(string paymentIntentId, bool flag)
		{
			var orderspec = new OrderSpecWithPaymentIntentId(paymentIntentId);
			var order = await _unitOfWork.Repository<Order>().GetAsyncWithSpec(orderspec);

			if (flag)
			{
				order.Status = OrderStatus.PaymentReceived;
			}
			else
			{
				order.Status = OrderStatus.PaymentFailed;
			}
			_unitOfWork.Repository<Order>().update(order);
			await _unitOfWork.Complete();

			return order;


		}
	}
}
