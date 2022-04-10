using Android.Content.Res;
using FoodOrderingApp.Models;
using FoodOrderingApp.Services.IServices;
using FoodOrderingApp.Views;
using Newtonsoft.Json;
using Nhom7_FoodOrderingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin;

namespace FoodOrderingApp.ViewModels
{
    public class DetailPageViewModel:BaseViewModel
    {
        ICartService cartService = DependencyService.Get<ICartService>();
        IFoodService foodService = DependencyService.Get<IFoodService>();

        public DetailPageViewModel()
        {
            PopDetailPageCommand = new Command(async () => await ExecutePopDetailPageCommand());
            IncreaseQuantCommand = new Command(ExecuteIncreaseQuantCommand);
            DecreaseQuantCommand = new Command(ExecuteDecreaseQuantCommand);
            ConvertToFavoriteCommand = new Command(async () => await ExecuteConvertToFavoriteCommand());
            AddToCartCommand = new Command(async () => await ExecuteAddToCartCommand());
            OpenRatingPopupCommand = new Command(ExecuteOpenRatingPopupCommand);
            Quant = 1;
        }

        FoodData _foods;
        public FoodData FoodData
        {
            get { return _foods; }
            set
            {
                _foods = value;
                OnPropertyChanged();
            }
        }
        //public INavigation Navigation { get; set; }
        public Command PopDetailPageCommand { get; }
        public Command IncreaseQuantCommand { get; }
        public Command DecreaseQuantCommand { get; }
        public Command AddToCartCommand { get; }
        public Command OpenRatingPopupCommand { get; }
        public Command ConvertToFavoriteCommand { get; }

        private int _quant;
        public int Quant
        {
            get { return _quant; }
            set { SetProperty(ref _quant, value); }
        }
        private string _heartImg;
        public string HeartImg
        {
            get { return _heartImg; }
            set { SetProperty(ref _heartImg, value); }
        }
        private int _price;
        public int Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }
        private async Task ExecutePopDetailPageCommand()
        {
            var vm = new HomePageViewModel();
            vm.GetFoodByCategory(0, 4);
            //await Application.Current.MainPage.Navigation.PopModalAsync();
            await Application.Current.MainPage.Navigation.PushModalAsync(new HomePage());
        }
        private void ExecuteIncreaseQuantCommand()
        {
            Quant += 1;
            Price = Quant * FoodData.price;
        }
        private async Task ExecuteConvertToFavoriteCommand()
        {
            if (HeartImg == "red_heart.png")
            {
                HeartImg = "black_heart.png";
            }
            else
            {
                HeartImg = "red_heart.png";
            }
            var result = await foodService.switchFavourite(FoodData.id);
            var str = JsonConvert.SerializeObject(result);
            Console.WriteLine("90"+str);
        }
        private void ExecuteDecreaseQuantCommand()
        {
            if (Quant > 1)
            {
                Quant -= 1;
                Price = Quant * FoodData.price;
            }
        }
        private async Task ExecuteAddToCartCommand()
        {
            var result = await cartService.addToCart(FoodData.id, Quant);
            Console.WriteLine("76;;;;"+ FoodData.id+";;;;"+Quant+"'''''"+ JsonConvert.SerializeObject(result));
            if(result != null)
            {
                if(result.status == 1)
                {
                    await Application.Current.MainPage.DisplayAlert("Notification", "Your food is added to cart", "OK");
                    //await Application.Current.MainPage.Navigation.PushModalAsync(new CartPage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Network Connection Error", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Network Connection Error", "OK");
            }
        }
        private void ExecuteOpenRatingPopupCommand()
        {
            var popup = new RatingPopup();
            popup.Dismissed += (s, e) =>
            {
                Console.WriteLine("129"+popup.getRatingStar());
                var result = foodService.addRatingFood(FoodData.id, popup.getRatingStar());
            };
             Application.Current.MainPage.Navigation.ShowPopup(popup);
        }


    }
}
