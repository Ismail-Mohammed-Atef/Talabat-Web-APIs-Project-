using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Models.Orders;
using Talabat.Core.Repository.Interfaces;
using Talabat.Core.Services.Interfaces;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Services
{
	public class OrderService : IOrderService
	{
		public IBasketRepository _BasketRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IpaymentService _paymentService;

		public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork , IpaymentService paymentService)
        {
			_BasketRepository = basketRepository;
			_unitOfWork = unitOfWork;
			_paymentService = paymentService;
		}


		public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address address)
		{
			var basket = await _BasketRepository.GetBasket(basketId);
			var orderitems = new List<OrderItem>();
			foreach (var item in basket.Items)
			{
				var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
				var orderproductitem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);

				var orderItem = new OrderItem(orderproductitem, item.Price, item.Quantity);


				orderitems.Add(orderItem);
			}

			var subtotal = orderitems.Sum(io=>io.Price*io.Quantity);

			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

			var spec = new OrderSpecWithPaymentIntentId(basket.PaymentIntentId);
			var execorder = await _unitOfWork.Repository<Order>().GetAsyncWithSpec(spec);

			if(execorder != null)
			{
				_unitOfWork.Repository<Order>().delete(execorder);

				basket = await _paymentService.CreateOrUpdatePaymentMethod(basketId);

			}


			var order = new Order(buyerEmail, address, deliveryMethod ,orderitems, subtotal,basket.PaymentIntentId);

			await _unitOfWork.Repository<Order>().Add(order);

			var count = await _unitOfWork.Complete();
			if(count <= 0) 
			{
				return null;
			}

			return order;
		}

		public async Task<Order?> GetOrderForSpecificUserAsync(string buyerEmail, int orderId)
		{
			var orderspec = new OrderSpecification(buyerEmail,orderId);
			var order = await _unitOfWork.Repository<Order>().GetAsyncWithSpec(orderspec);
			if (order == null) return null;
			return order;
		}

		public async Task<IReadOnlyList<Order>?> GetOrdersOfSpecificUserAsync(string buyerEmail)
		{
			var orderspec = new OrderSpecification(buyerEmail);

			var orders = await _unitOfWork.Repository<Order>().GetAllAsyncWithSpec(orderspec);
			return orders;

		}
	}
}
