using FoodOrderingApp.Models;
using FoodOrderingApp.Services;
using FoodOrderingApp.Services.IServices;
using FoodOrderingApp.Views;
using Newtonsoft.Json;
using Nhom7_FoodOrderingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FoodOrderingApp.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        ICategoryService categoryService= DependencyService.Get<ICategoryService>();
        IFoodService foodService= DependencyService.Get<IFoodService>();
        ICartService cartService= DependencyService.Get<ICartService>();
        public HomePageViewModel()
        {
            //Navigation = navigation;
            NavigateToDetailPageCommand = new Command<FoodData>(async (model) => await ExecuteNavigateToDetailPageCommand(model));
            SelectGroupCommand = new Command<CategoryData>((model) => ExecuteSelectGroupCommand(model));
            NavigateToCartPageCommand = new Command(async () => await ExecuteNavigateToCartPageCommand());
            NavigateToFavouritePageCommand = new Command(async () => await ExecuteNavigateToFavouritePageCommand());
            NavigateToSearchPageCommand = new Command<ObservableCollection<FoodData>>(async (models) => await  ExecuteNavigateToSearchPageCommand(models));
            OpenSidebarMenuCommand = new Command(ExecuteOpenSidebarMenuCommand);

            //Application.Current.MainPage.Navigation.ShowPopup(new MenuPopup());
            GetAllCatigories();
           GetFoodByCategory(0, 4);
            //SecureStorage.SetAsync("userId", "1");
            //SecureStorage.SetAsync("token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6NSwidXNlcm5hbWUiOiJuaGFuMyIsInBhc3N3b3JkIjoiJDJiJDEwJEJXL2lzYy9XNExXdVRqbU5TMDl6Lk8zLjNHODJiYUpHVzRqZTFJbU44bkg3RTlOeEkuN2t5IiwiZnVsbG5hbWUiOm51bGwsImVtYWlsIjpudWxsLCJwaG9uZSI6bnVsbCwiY3JlYXRlZEF0IjoiMjAyMi0wMy0xNFQwODoxMjoxMi4wMDBaIiwidXBkYXRlZEF0IjoiMjAyMi0wMy0xNFQwODoxMjoxMi4wMDBaIiwiaWF0IjoxNjQ4MjIyNzY0LCJleHAiOjE2NDg4Mjc1NjR9.Yu78j4T4NxUnxk3Rmbe2lzm_0Dkqzqx2wOwA_7vPsu0");
        }

        public Command SelectGroupCommand { get; }

        public Command NavigateToDetailPageCommand { get; }
        public Command NavigateToFavouritePageCommand { get; }
        public Command NavigateToCartPageCommand { get; }
        public Command NavigateToSearchPageCommand { get; }
        public Command OpenSidebarMenuCommand { get; }
        
        private String _description = "Add a joy of best Taste";
        public String Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private ObservableCollection<CategoryData> _categories;
        public ObservableCollection<CategoryData> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }
        private ObservableCollection<FoodData> _foodsRecommend;
        public ObservableCollection<FoodData> FoodsRecommend
        {
            get { return _foodsRecommend; }
            set { SetProperty(ref _foodsRecommend, value); }
        }
        private ObservableCollection<FoodData> _foodsPopular;
        public ObservableCollection<FoodData> FoodsPopular
        {
            get { return _foodsPopular; }
            set { SetProperty(ref _foodsPopular, value); }
        }
        async void GetAllCatigories()
        {
            var result = await categoryService.getAllCategories();
            ObservableCollection<CategoryData> ct = new ObservableCollection<CategoryData>();
            if (result!=null)
            {
                foreach (var category in result.data)
                {
                    var categoryData = new CategoryData()
                    {
                        id = category.id,
                        name = category.name,
                        description = category.description,
                        selected = false,
                        backgroundColor = Color.FromHex("#FFFFFF"),
                        image = category.image,
                    };
                    ct.Add(categoryData);
                }
                CategoryData allCate = new CategoryData()
                {
                    id = 0,
                    name = "All",
                    description = "Add a joy of best Taste",
                    selected = true,
                    backgroundColor = Color.FromHex("#FF8920"),
                    image = "https://firebasestorage.googleapis.com/v0/b/foodorderingapp-36be0.appspot.com/o/all.jpg?alt=media&token=d2abb8c2-cbf4-4b7e-aa98-e5573d1e45ed",
                };
                ct.Insert(0, allCate);
                //ct[0].selected = true;
                //ct[0].backgroundColor = Color.FromHex("#FF8920");
                Categories = ct;
            }
           
        }
        public async void GetFoodByCategory(int categoryId, int number)
        {
            var result = await foodService.getFoodByCategory(categoryId, number);
            ObservableCollection<FoodData> fd = new ObservableCollection<FoodData>();
            var userId = await SecureStorage.GetAsync("userId");
            Console.WriteLine("116" + JsonConvert.SerializeObject(result));
            if (result!=null&&result.status==1)
            {
                foreach(var food in result.data.data)
                {
                    Console.WriteLine("120"+ JsonConvert.SerializeObject(food.user_like));
                    if (food.user_like !=null&& food.user_like.ToList().FindIndex(e => e.id == Int32.Parse(userId)) > -1)
                    {
                        food.HeartImg = "red_heart.png";
                    }
                }
                FoodsRecommend = new ObservableCollection<FoodData>(result.data.data.OrderByDescending(e => e.name));
                FoodsPopular = new ObservableCollection<FoodData>(result.data.data.OrderByDescending(e => e.rate));
            }
        }
        private async Task ExecuteNavigateToDetailPageCommand(FoodData model)
        {
            if(model != null)
            {
                var viewModel = new DetailPageViewModel { FoodData=model, Price=model.price, HeartImg = model.HeartImg};
                var detailsPage = new DetailPage { BindingContext = viewModel };

                await Application.Current.MainPage.Navigation.PushModalAsync(detailsPage);
            }
        }
        private async Task ExecuteNavigateToCartPageCommand()
        {
            //var result = await cartService.getCart();
            //if (result != null)
            //{
            //    var totalPrice = 0;
            //    var totalQuantity = 0;
            //    foreach(var items in result.data.order_foods)
            //    {
            //        totalQuantity=totalQuantity+items.quantity;
            //        totalPrice=totalPrice+items.price;
            //    }
            //    var viewModel = new CartPageViewModel { OrderFood = result.data.order_foods, TotalPrice=totalPrice, TotalQuantity = totalQuantity };
            //    var cartPage = new CartPage { BindingContext = viewModel };
            //    await Application.Current.MainPage.Navigation.PushModalAsync(cartPage);
            //}
            await Application.Current.MainPage.Navigation.PushModalAsync(new CartPage());
        }
        private async Task ExecuteNavigateToFavouritePageCommand()
        {
            var result = await foodService.getFoodFavourite();
            if (result != null)
            {
                //var viewModel = new CartPageViewModel { OrderFood = result.data.order_foods, TotalPrice=totalPrice, TotalQuantity = totalQuantity };

                Console.WriteLine("23" + JsonConvert.SerializeObject(result));
                await Application.Current.MainPage.Navigation.PushModalAsync(new FavouritePage(result.data.data));
            }
        }
        private async Task ExecuteNavigateToSearchPageCommand(ObservableCollection<FoodData> models)
        {
            if (models != null)
            { 
                //{
                //    var viewModel = new SearchPageViewModel { FoodData = models };
                //    var searchPage = new SearchPage() { BindingContext = viewModel };

                //    await Application.Current.MainPage.Navigation.PushModalAsync(searchPage);
                await Application.Current.MainPage.Navigation.PushModalAsync(new SearchPage(models));
            }
        }

        private void ExecuteOpenSidebarMenuCommand()
        {
            //var popup = new RatingPopup();
            //popup.Dismissed += (s, e) =>
            //{
            //    Console.WriteLine("129" + popup.getRatingStar());
            //    var result = foodService.addRatingFood(FoodData.id, popup.getRatingStar());
            //};
            Application.Current.MainPage.Navigation.ShowPopup(new MenuPopup());
        }

        private async void ExecuteSelectGroupCommand(CategoryData model)
        {
            var index = Categories
                    .ToList()
                    .FindIndex(p => p.id == model.id);
            if (index > -1)
            {
                UnselectGroupItems();
                Categories[index].selected = true;
                Categories[index].backgroundColor = Color.FromHex("#FF8920");
                Description = (string)Categories[index].description;
                GetFoodByCategory(model.id, 4);
            }
            Console.WriteLine("161"+model.id);
            var result = await foodService.getFoodByCategory(model.id, 4);
            ObservableCollection<FoodData> fd = new ObservableCollection<FoodData>();
            var userId = await SecureStorage.GetAsync("userId");
            if (result.status == 1)
            {
                foreach (var food in result.data.data)
                {
                    if (food.user_like != null&&food.user_like.ToList().FindIndex(e => e.id == Int32.Parse(userId)) > -1)
                    {
                        food.HeartImg = "red_heart.png";
                    }
                }
                FoodsRecommend = new ObservableCollection<FoodData>(result.data.data.OrderByDescending(e => e.price));
                //FoodsPopular = new ObservableCollection<FoodData>(result.data.data.OrderByDescending(e => e.rate));
            }
        }

        void UnselectGroupItems()
        {
            Categories.ForEach((item) =>
            {
                item.selected = false;
                item.backgroundColor = Color.FromHex("#FFFFFF");
            });
        }
    }
}
