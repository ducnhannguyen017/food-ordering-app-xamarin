using FoodOrderingApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodOrderingApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage(ObservableCollection<FoodData> models)
        {
            InitializeComponent();
            BindingContext = this;
            FoodData = models;
            Console.WriteLine("23" + JsonConvert.SerializeObject(FoodData));
        }
        ObservableCollection<FoodData> _foods;
        public ObservableCollection<FoodData> FoodData
        {
            get { return _foods; }
            set
            {
                _foods = value;
                OnPropertyChanged();
            }
        }
        //private void Search_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                listFoodsPopular.ItemsSource = new ObservableCollection<FoodData>(FoodData.Where(ele => ele.name.ToLower().Contains(searchBar.Text)));
            }
        }
    }
}