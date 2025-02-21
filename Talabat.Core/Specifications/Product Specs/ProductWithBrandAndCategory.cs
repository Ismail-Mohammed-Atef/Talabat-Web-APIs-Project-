using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications.Product_Specs
{
	public class ProductWithBrandAndCategory : BaseSpecification<Product>
	{
        public ProductWithBrandAndCategory(ProductSpecs specs) : base
        (
            p =>
            (string.IsNullOrEmpty(specs.Search) || p.Name.ToLower().Contains(specs.Search))
            &&
			(!specs.BrandId.HasValue || specs.BrandId.Value == p.BrandId)
			&&
			(!specs.TypeId.HasValue || specs.TypeId.Value == p.TypeId)

		)
		{
            Includes.Add(p=>p.ProductBrand);
            Includes.Add(p=>p.ProductType);

            if (!string.IsNullOrEmpty(specs.sort))
            {
                switch (specs.sort)
                {
                    case "PriceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }


            AddPagination(specs.PageSize,(specs.PageSize*(specs.PageIndex-1)));
        }

        public ProductWithBrandAndCategory(int id) : base(p=>p.Id == id)
        {
			Includes.Add(p => p.ProductBrand);
			Includes.Add(p => p.ProductType);
		}
    }
}
