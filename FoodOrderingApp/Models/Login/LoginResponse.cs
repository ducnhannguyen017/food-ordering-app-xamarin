using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrderingApp.Models.Login
{
    public class UserData
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class LoginResponseData
    {
        public string token { get; set; }
        public UserData data { get; set; }
    }
    public class LoginResponse
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public LoginResponseData data { get; set; }
    }


}
