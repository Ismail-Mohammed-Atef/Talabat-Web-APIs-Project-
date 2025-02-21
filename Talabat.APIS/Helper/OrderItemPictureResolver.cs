using AutoMapper;
using Talabat.APIS.Dtos;
using Talabat.Core.Models.Orders;

namespace Talabat.APIS.Helper
{
	public class OrderItemPictureResolver : IValueResolver<OrderItem, OrderItemsDto, string>
	{
		private readonly IConfiguration _configuration;

		public OrderItemPictureResolver(IConfiguration configuration)
        {
			_configuration = configuration;
		}
       
		public string Resolve(OrderItem source, OrderItemsDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.Product.PictureUrl))
			{
				return $"{_configuration["BaseUrl"]}/{source.Product.PictureUrl}";
			}
			return string.Empty;
		}
	}
}
