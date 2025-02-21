using Talabat.Core.Models.Orders;

namespace Talabat.APIS.Dtos
{
	public class OrderToReturnDto
	{
		public int Id { get; set; }
		public string BuyerEmail { get; set; }
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
		public string Status { get; set; }
		public Address ShippingAddress { get; set; }
		public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
		public decimal SubTotal { get; set; }
		public decimal Total {  get; set; }
		public string PaymentIntentId { get; set; } = string.Empty;

	}
}
