using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppinngCartItem> ShoppinngCartItems { get; set; } = new List<ShoppinngCartItem>();

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }
    }
}
