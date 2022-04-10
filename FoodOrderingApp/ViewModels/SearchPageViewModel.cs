using FoodOrderingApp.Models;
using FoodOrderingApp.Views;
using Newtonsoft.Json;
using Nhom7_FoodOrderingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodOrderingApp.ViewModels
{
    public class SearchPageViewModel : BaseViewModel
    {
        public SearchPageViewModel() {
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

        public Command SearchCommand { get; }
        
    }
}
