using Talabat.Core.Entites;

namespace Talabat.APIS.Dtos
{
	public class ProductToDto
	{
        public int Id { get; set; }
        public string Name { get; set; }
		public string Description { get; set; }
		public string PictureUrl { get; set; }
		public decimal Price { get; set; }

		public int BrandId { get; set; } //FK

		public string Brand { get; set; }

		public int TypeId { get; set; } //FK
		public string Type { get; set; }

	}
}
