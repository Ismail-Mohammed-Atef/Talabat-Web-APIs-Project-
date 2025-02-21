using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications.Product_Specs
{
	public class ProductSpecCount : BaseSpecification<Product>
	{
        public ProductSpecCount(ProductSpecs specs) : base
		(
			p =>

			(!specs.BrandId.HasValue || specs.BrandId.Value == p.BrandId)
			&&
			(!specs.TypeId.HasValue || specs.TypeId.Value == p.TypeId)
		)
		{
            
        }
    }
}
