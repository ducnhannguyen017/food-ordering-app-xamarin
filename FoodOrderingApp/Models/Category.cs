using Nhom7_FoodOrderingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace FoodOrderingApp.Models
{
    public class CategoryData : BaseViewModel
    {
        public int id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string image { get; set; }

        private bool _selected;
        public bool selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }
        private Color _backgroundColor;
        public Color backgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }
    }

    public class Category
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public ObservableCollection<CategoryData> data { get; set; }
    }
    public class CategoryResponse
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public CategoryData data { get; set; }
    }

    public class CategoryAddRequest
    {
        public string name { get; set; }
        public string description { get; set; }
    }
}
