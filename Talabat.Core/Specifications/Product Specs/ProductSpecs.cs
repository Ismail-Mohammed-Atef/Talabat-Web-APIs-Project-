using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Product_Specs
{
	public class ProductSpecs
	{
        private string? search { get; set; }

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }
        public int PageIndex { get; set; } = 1;
        private const int maxPageSize = 5;
        private int pageSize = maxPageSize;

        public int PageSize {
            get { return pageSize; }
            set { pageSize = value > maxPageSize ? maxPageSize : value; }
        }
        public string? sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
    }
}
