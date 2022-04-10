using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Models.Register
{
    public class RegisterRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
