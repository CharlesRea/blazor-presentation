using System;
using System.Collections.Generic;
using BlazorBites.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorBites.Server
{
    [ApiController]
    [Route("api")]
    public class OrderController : ControllerBase
    {
        [HttpGet("menu")]
        public IEnumerable<Dish> GetMenu()
        {
            return new[]
            {
                new Dish
                {
                    DishId = "burger",
                    Name = "Biggest Burger",
                    Price = 11.99m,
                },
                new Dish
                {
                    DishId = "pizza",
                    Name = "Perfect Pizza",
                    Price = 13.50m,
                },
                new Dish
                {
                    DishId = "tacos",
                    Name = "Tasty Tacos",
                    Price = 6.00m,
                },
            };
        }

        [HttpPost("order")]
        public ConfirmedOrder PlaceOrder(Order order)
        {
            var random = new Random();
            return new ConfirmedOrder
            {
                ExpectedDeliveryMinutes = random.Next(10, 30),
            };
        }
    }
}
