using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites
{
	public class Product : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string PictureUrl { get; set; }
		public decimal Price { get; set; }

		[ForeignKey(nameof(ProductBrand))]
		public int BrandId { get; set; } //FK
		public ProductBrand ProductBrand { get; set; }

		[ForeignKey(nameof(ProductType))]
		public int TypeId { get; set; } //FK
		public ProductType ProductType { get; set; }


	}
}
