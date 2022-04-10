using FoodOrderingApp.Services;
using FoodOrderingApp.Services.IServices;
using FoodOrderingApp.Views;
using FoodOrderingApp.Views.Admin;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodOrderingApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<ICategoryService, CategoryService>();
            DependencyService.Register<IFoodService, FoodService>();
            DependencyService.Register<ICartService, CartService>();
            DependencyService.Register<IUserService, UserService>();
            DependencyService.Register<IOrderService, OrderService>();
            DependencyService.Register<IPaymentService, PaymentService>();
            //Device.SetFlags(new[] { "Shapes_Experimental" });
            Device.SetFlags(new string[] { "CollectionView_Experimental", "Expander_Experimental", "Shapes_Experimental" });
            //MainPage = new AppShell();
            //MainPage = new HomePage();
            //MainPage = new CategoryPage();
            //MainPage = new NavigationPage(new HomeAdminPage());
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
