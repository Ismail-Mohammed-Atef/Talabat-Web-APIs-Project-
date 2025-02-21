using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Models.Orders;

namespace Talabat.Repository.Data
{
	public class StoreDbContextDataSeed
	{
		public static async Task SeedAsync(StoreDbContext context)
		{
			if (context.ProductBrands.Count() == 0)
			{
				var brandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

				if (brands?.Count() > 0)
				{
					foreach (var brand in brands)
					{
						context.Set<ProductBrand>().Add(brand);
					}
					await context.SaveChangesAsync();
				}
			}
			if (context.ProductTypes.Count() == 0)
			{
				var categoryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
				var categories = JsonSerializer.Deserialize<List<ProductType>>(categoryData);

				if (categories?.Count() > 0)
				{
					foreach (var category in categories)
					{
						context.Set<ProductType>().Add(category);
					}
					await context.SaveChangesAsync();
				}
			}
			if (context.Products.Count() == 0)
			{
				var productData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productData);

				if (products?.Count() > 0)
				{
					foreach (var product in products)
					{
						context.Set<Product>().Add(product);
					}
					await context.SaveChangesAsync();
				}
			}

			if(context.DeliveryMethods.Count() == 0)
			{
				var deliveryMethodsjson = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
				var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsjson);

				if(deliveryMethods?.Count() > 0)
				{
					foreach(var deliveryMethod in deliveryMethods)
					{
						context.DeliveryMethods.Add(deliveryMethod);
					}
					await context.SaveChangesAsync();
				}
			}

		}
	}
}

