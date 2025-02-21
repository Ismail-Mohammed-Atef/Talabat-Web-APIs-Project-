using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repository.Interfaces;


namespace Talabat.Repository.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _dataBase;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _dataBase = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasket(string id)
		{
			return await _dataBase.KeyDeleteAsync(id);
		}

		public async Task<CustomerBasket?> GetBasket(string id)
		{
			var basket = await _dataBase.StringGetAsync(id);
			return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
		}

		public async Task<CustomerBasket?> UpdateBasket(CustomerBasket basket)
		{
			 var CreatedOrUpdated = await _dataBase.StringSetAsync(basket.Id,JsonSerializer.Serialize(basket),TimeSpan.FromDays(30));

			if (CreatedOrUpdated is false)
				return null;

			return await GetBasket(basket.Id);
		}
	}
}
