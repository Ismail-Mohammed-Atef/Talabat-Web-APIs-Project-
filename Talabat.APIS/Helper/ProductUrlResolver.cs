using AutoMapper;
using Talabat.APIS.Dtos;
using Talabat.Core.Entites;

namespace Talabat.APIS.Helper
{
	public class ProductUrlResolver : IValueResolver<Product, ProductToDto, string>
	{
		private readonly IConfiguration _configuration;

		public ProductUrlResolver(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public string Resolve(Product source, ProductToDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.PictureUrl))
			{
				return $"{_configuration["BaseUrl"]}/{source.PictureUrl}";
			}
			return string.Empty;
		}
	}
}
