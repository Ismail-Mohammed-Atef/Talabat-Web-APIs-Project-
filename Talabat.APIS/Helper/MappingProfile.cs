using AutoMapper;
using Talabat.APIS.Dtos;
using Talabat.Core.Entites;
using useraddress = Talabat.Core.Entites.Identity.Address;
using orderaddress = Talabat.Core.Models.Orders.Address;
using Talabat.Core.Models.Orders;
using Microsoft.OpenApi.Extensions;

namespace Talabat.APIS.Helper
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Product, ProductToDto>().
                ForMember(d => d.Brand, o => o.MapFrom(p => p.ProductBrand.Name))
                .ForMember(d => d.Type, o => o.MapFrom(p => p.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<useraddress, AddressDto>().ReverseMap();
            CreateMap<orderaddress, AddressDto>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>().ForMember(o => o.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(o => o.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));


            CreateMap<OrderItem,OrderItemsDto>().ForMember(oi=>oi.ProductName,o=>o.MapFrom(s=>s.Product.ProductName))
                .ForMember(oi=>oi.PictureUrl,o=>o.MapFrom<OrderItemPictureResolver>())
                .ForMember(oi=>oi.ProductId,o=>o.MapFrom(s=>s.Product.ProductId));

            CreateMap<CustomerBasket,CustomerBasketDto>().ReverseMap();
		}
    }
}
