using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CartReponseData
    {
        public int id { get; set; }
        public object note { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int order_id { get; set; }
        public int food_id { get; set; }
    }

    public class CartResponse
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public CartReponseData data { get; set; }
    }


}
