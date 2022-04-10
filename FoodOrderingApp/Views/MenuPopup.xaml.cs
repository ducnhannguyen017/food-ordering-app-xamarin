using FoodOrderingApp.Models;
using FoodOrderingApp.Services.IServices;
using FoodOrderingApp.ViewModels;
using FoodOrderingApp.Views.Admin;
using Newtonsoft.Json;
using Nhom7_FoodOrderingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodOrderingApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPopup : Xamarin.CommunityToolkit.UI.Views.Popup
    {
        ICartService cartService = DependencyService.Get<ICartService>();
        IFoodService foodService = DependencyService.Get<IFoodService>();

        public MenuPopup()
        {
            InitializeComponent();
            NavigationCommand = new Command<MenuModel>(async (model) => await ExecuteNavigationCommand(model));
            //SecureStorage.SetAsync("role", "user");
            getMenuList();
            Console.WriteLine("20"+ JsonConvert.SerializeObject(MenuList));
            BindingContext = this;

        }
        public Command NavigationCommand { get;}
        public ObservableCollection<MenuModel> MenuList { get; set; }
        public string fullname { get; set; }
        private async void getMenuList()
        {
            var role = await SecureStorage.GetAsync("role");
            if (role == "user")
            {
                MenuList = getMenuListUser();
            }
            else
            {
                MenuList = getMenuListSaler();
            }
            fullname = await SecureStorage.GetAsync("fullname");

        }
        private ObservableCollection<MenuModel> getMenuListSaler()
        {
            return new ObservableCollection<MenuModel>
            {
                new MenuModel
                {
                    name ="Category",
                    icon ="category.png",
                },
                new MenuModel
                {
                    name ="Order",
                    icon ="order.png",
                },
                new MenuModel
                {
                    name ="Food",
                    icon ="food.jpg",
                },
                new MenuModel
                {
                    name ="Logout",
                    icon ="logout.png",
                },
            };
        }
        private ObservableCollection<MenuModel> getMenuListUser()
        {
            return new ObservableCollection<MenuModel>
            {
                new MenuModel
                {
                    name ="Home",
                    icon ="home.png",
                },
                new MenuModel
                {
                    name ="Favourite",
                    icon ="favourite.png",

                },
                new MenuModel
                {
                    name ="Search",
                    icon ="search.jpg",

                },
                new MenuModel
                {
                    name ="Cart",
                    icon ="shopping_cart.png",
                },
                new MenuModel
                {
                    name ="Logout",
                    icon ="logout.png",
                },
            };
        }
        private async Task ExecuteNavigationCommand(MenuModel model)
        {
            Console.WriteLine("71"+ JsonConvert.SerializeObject(model));
            switch (model.name)
            {
                case "Home":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new HomePage());
                    break;
                case "Favourite":
                    var result = await foodService.getFoodFavourite();
                    if (result != null)
                    {
                        Console.WriteLine("23" + JsonConvert.SerializeObject(result));
                        await Application.Current.MainPage.Navigation.PushModalAsync(new FavouritePage(result.data.data));
                    }
                    break;
                case "Search":
                    var resultSearch = await foodService.getFoodByCategory(0, 4);
                    ObservableCollection<FoodData> fd = new ObservableCollection<FoodData>();
                    var userId = await SecureStorage.GetAsync("userId");
                    Console.WriteLine("116" + JsonConvert.SerializeObject(resultSearch));
                    if (resultSearch != null)
                    {
                        foreach (var food in resultSearch.data.data)
                        {
                            Console.WriteLine("120" + JsonConvert.SerializeObject(food.user_like));
                            if (food.user_like != null && food.user_like.ToList().FindIndex(e => e.id == Int32.Parse(userId)) > -1)
                            {
                                food.HeartImg = "red_heart.png";
                            }
                        }
                        ObservableCollection<FoodData> modelSearch = new ObservableCollection<FoodData>(resultSearch.data.data);
                        await Application.Current.MainPage.Navigation.PushModalAsync(new SearchPage(modelSearch));
                    }
                    break;
                case "Cart":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new CartPage());
                    break;
                case "Logout":
                    await SecureStorage.SetAsync("token", "");
                    await SecureStorage.SetAsync("userId", "");
                    await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
                    break;
                case "Category":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new CategoryPage());
                    break;
                case "Order":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new OrderPage());
                    break;
                case "Food":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new FoodPage());
                    break;

            }
            Dismiss("dismissed");
        }

    }
    public class MenuModel
    {
        public string name { get; set; }
        public string icon { get; set; }
    }
}