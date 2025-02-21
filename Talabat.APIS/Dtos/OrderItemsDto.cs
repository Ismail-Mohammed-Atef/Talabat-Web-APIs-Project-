using Talabat.Core.Models.Orders;

namespace Talabat.APIS.Dtos
{
	public class OrderItemsDto
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string PictureUrl { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
