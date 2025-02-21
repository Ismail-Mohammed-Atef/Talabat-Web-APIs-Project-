using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Dtos;
using Talabat.APIS.Errors;
using Talabat.APIS.Helper;
using Talabat.Core.Entites;
using Talabat.Core.Repository.Interfaces;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.APIS.Controllers
{

	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productRepository;
		private readonly IMapper _mapper;
		private readonly IGenericRepository<ProductBrand> _brandRepository;
		private readonly IGenericRepository<ProductType> _categoriesRepository;

		public ProductsController(IGenericRepository<Product> productRepository, IMapper mapper, IGenericRepository<ProductBrand> brandRepository, IGenericRepository<ProductType> categoriesRepository)
		{
			_productRepository = productRepository;
			_mapper = mapper;
			_brandRepository = brandRepository;
			_categoriesRepository = categoriesRepository;
		}


		[HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToDto>>> GetProducts([FromQuery] ProductSpecs specs)
		{
			//var products = await _productRepository.GetAllAsync();
			var spec = new ProductWithBrandAndCategory(specs);
			var countspec = new ProductSpecCount(specs);
			var products = await _productRepository.GetAllAsyncWithSpec(spec);

			var productDto = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToDto>>(products);

			int count = await _productRepository.GetCountAsync(countspec);
			var product = new Pagination<ProductToDto>(specs.PageIndex, specs.PageSize, count, productDto);
			return Ok(product);
		}


		[ProducesResponseType(typeof(ProductToDto),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetProductById(int id)
		{
			var spec = new ProductWithBrandAndCategory(id);
			var product = await _productRepository.GetAsyncWithSpec(spec);

			if (product == null)
				return NotFound(new ApiResponse(404));

			var productDto = _mapper.Map<Product,ProductToDto>(product);

			return Ok(productDto);
		}


		[HttpGet("brands")]
		public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
		{
			var brands = await  _brandRepository.GetAllAsync();
			return  Ok(brands);
		}


		[HttpGet("types")]
		public async Task<ActionResult<IEnumerable<ProductType>>> GetCategories()
		{
			var categories = await _categoriesRepository.GetAllAsync();
			return Ok(categories);
		}
	}

}
