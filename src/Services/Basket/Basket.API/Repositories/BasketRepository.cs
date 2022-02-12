using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            _redisCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }
        public async Task DeleteBasket(ShoppingCart shoppingCart)
        {
            await _redisCache.RemoveAsync(shoppingCart.UserName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
           var basket = await _redisCache.GetStringAsync(userName);
            if (String.IsNullOrEmpty(basket))
                return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
        {
            await _redisCache.SetStringAsync(shoppingCart.UserName, JsonConvert.SerializeObject(shoppingCart));
            
            return await GetBasket(shoppingCart.UserName);
        }
    }
}
