using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Models.Orders;

namespace Talabat.Core.Services.Interfaces
{
	public interface IpaymentService
	{
		Task<CustomerBasket?> CreateOrUpdatePaymentMethod(string basketId);

		Task<Order> UpdatePaymentIntentToSucceedOrFailed(string paymentIntentId,bool flag);
	}
}
