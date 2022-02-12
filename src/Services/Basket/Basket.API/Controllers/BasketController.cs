using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository repository)
        {
            _basketRepository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetBasket(string userName)
        {
            ShoppingCart shoppingCart = await _basketRepository.GetBasket(userName);
            return Ok(shoppingCart ?? new ShoppingCart(userName));             
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBasket(ShoppingCart shoppingCart)
        {
            await _basketRepository.UpdateBasket(shoppingCart);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(ShoppingCart shoppingCart)
        {
            await _basketRepository.DeleteBasket(shoppingCart);
            return Ok();
        }
    }
}
