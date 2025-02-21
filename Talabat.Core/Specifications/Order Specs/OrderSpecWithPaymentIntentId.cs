using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Orders;

namespace Talabat.Core.Specifications.Order_Specs
{
	public class OrderSpecWithPaymentIntentId : BaseSpecification<Order>
	{
        public OrderSpecWithPaymentIntentId(string paymentIntentId) : base(o=>o.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
