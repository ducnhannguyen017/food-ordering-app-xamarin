using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Models
{
    public class CartRequest
    {
        public int food_id { get; set; }
        public int quantity { get; set; }
    }
    public class CartUpdateRequest
    {
        public int id { get; set; }
        public int quantity { get; set; }
    }
}
