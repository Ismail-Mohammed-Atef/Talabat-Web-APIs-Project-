using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Orders;

namespace Talabat.Core.Services.Interfaces
{
	public interface IOrderService
	{
		Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address address);
		Task<IReadOnlyList<Order>?> GetOrdersOfSpecificUserAsync(string buyerEmail);

		Task<Order?> GetOrderForSpecificUserAsync(string buyerEmail , int orderId);
	}
}
