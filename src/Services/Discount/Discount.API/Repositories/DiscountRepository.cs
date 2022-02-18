using Discount.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration config)
        {
            _configuration = config;
        }
        public async Task<Coupon> GetDiscount(string ProductName)
        {
            using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));            
            
                var coupon = await conn.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = ProductName });
            if (coupon == null)
            {
                return new Coupon { ProductName = "No discount", Description = "No discount desribed", Amount = 0 };
            }
            return coupon;
            
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
                 
            using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            int affected = await conn.ExecuteAsync
              ("Insert into Coupon(ProductName, Description, Amount) values(@ProductName, @Description, @Amount)",
              new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> DeleteDiscount(string ProductName)
        {
            using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            int affected = await conn.ExecuteAsync
             ("Delete from Coupon where  ProductName = @ProductName",
             new { ProductName =  ProductName  });

            if (affected == 0)
                return false;
            return true;
        }


        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await conn.ExecuteAsync
                   ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                           new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            if (affected == 0)
                return false;

            return true;
        }
    }
}
