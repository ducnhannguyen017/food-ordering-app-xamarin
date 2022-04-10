using FoodOrderingApp.Models.Login;
using Nhom7_FoodOrderingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FoodOrderingApp.Models
{
    public class FoodInOrder
    {
        public int id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string image { get; set; }
        public int status { get; set; }
        public int rate { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int category_id { get; set; } = 0;
    }
    public class OrderFood:BaseViewModel
    {
        public int id { get; set; }
        public string note { get; set; }
        private int _quantity;
        public int quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }
        public int price { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int order_id { get; set; }
        public int food_id { get; set; }
        public FoodInOrder food { get; set; }
    }

    public class CartData
    {
        public int id { get; set; }
        public string address { get; set; }
        public int status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int user_id { get; set; }
        public ObservableCollection<OrderFood> order_foods { get; set; }
        public UserData user {get; set;}
    }

    public class Cart
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public CartData data { get; set; }
    }
    public class OrderAdmin
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public OrderAdminData data { get; set; }
    }
    public class OrderAdminData
    {
        public ObservableCollection<CartData> data { get; set; }
        public int total_page { get; set; }
        public int total { get; set; }
        public int current_page { get; set; }
    }
}
