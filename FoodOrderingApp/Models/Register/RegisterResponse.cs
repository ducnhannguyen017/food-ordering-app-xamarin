using FoodOrderingApp.Models.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Models.Register
{
    public class RegisterResponseData
    {
        public string token { get; set; }
        public UserData data { get; set; }
    }
    public class RegisterResponse
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public LoginResponseData data { get; set; }
    }
}
