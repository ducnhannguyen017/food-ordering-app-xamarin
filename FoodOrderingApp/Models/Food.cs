using Nhom7_FoodOrderingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FoodOrderingApp.Models
{
    public class Data
    {
        public ObservableCollection<FoodData> data { get; set; }

        public int total_page { get; set; }
        public int total { get; set; }
        public int current_page { get; set; }
    }
    public class FoodData:BaseViewModel
    {
        public int id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string image { get; set; }
        public int rate { get; set; }
        public int status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int category_id { get; set; }
        private string _heartImg = "black_heart.png";
        public string HeartImg
        {
            get { return _heartImg; }
            set { SetProperty(ref _heartImg, value); }
        }
        public ObservableCollection<UserLike> user_like { get; set; }
    }

    public class UserLike
    {
        public int id { get; set; }
        public Favourite favourite { get; set; }
    }
    public class Favourite
    {
        public string description { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int user_id { get; set; }
        public int food_id { get; set; }
    }

    public class Food
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
    public class FoodManipulationResponse
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public FoodData data { get; set; }
    }
}
