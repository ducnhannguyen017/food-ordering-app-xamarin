using FoodOrderingApp.Models;
using FoodOrderingApp.ViewModels;
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
    public partial class FavouritePage : ContentPage
    {
        public FavouritePage(ObservableCollection<FoodData> favouriteFoods)
        {
            InitializeComponent();
            BindingContext = this;
            NavigateToHomePageCommand = new Command(async () => await ExecuteNavigateToHomePageCommand());
            NavigateToDetailPageCommand = new Command<FoodData>(async (model) => await ExecuteNavigateToDetailPageCommand(model));

            Console.WriteLine("23" + JsonConvert.SerializeObject(favouriteFoods));

            FavouriteFoods = favouriteFoods;
        }

        public Command NavigateToDetailPageCommand { get; }
        public Command NavigateToHomePageCommand { get; }

        ObservableCollection<FoodData> _foods;
        public ObservableCollection<FoodData> FavouriteFoods
        {
            get { return _foods; }
            set
            {
                _foods = value;
                OnPropertyChanged();
            }
        }

        private async Task ExecuteNavigateToDetailPageCommand(FoodData model)
        {
            if (model != null)
            {
                var viewModel = new DetailPageViewModel { FoodData = model, Price = model.price, HeartImg = "red_heart.png" };
                var detailsPage = new DetailPage { BindingContext = viewModel };

                await Application.Current.MainPage.Navigation.PushModalAsync(detailsPage);
            }
        }
        private async Task ExecuteNavigateToHomePageCommand()
        {
            Console.WriteLine("56-");
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Console.WriteLine("56-");
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}