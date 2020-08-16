using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorBites.Shared
{
    public class Order
    {
        public IEnumerable<Dish> Dishes { get; set; } = new List<Dish>();

        [Required(ErrorMessage = "Please enter your name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        public string CustomerAddress { get; set; }
    }
}
