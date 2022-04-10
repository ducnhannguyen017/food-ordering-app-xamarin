using FoodOrderingApp.Models.Login;
using FoodOrderingApp.Services.IServices;
using FoodOrderingApp.Views.Admin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodOrderingApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : TabbedPage
    {
        IUserService userService = DependencyService.Get<IUserService>();

        public LoginPage()
        {
            InitializeComponent();
        }
        private async void LogInClicked(object sender, EventArgs e)
        {
            if(userRadioButton.IsChecked == true)
            {
                var result = await userService.userLogin(userName.Text, userPassword.Text);
                Console.WriteLine(JsonConvert.SerializeObject(result));
                if (result != null && result.status == 1)
                {
                    await SecureStorage.SetAsync("token", result.data.token);
                    await SecureStorage.SetAsync("userId", result.data.data.id.ToString());
                    await SecureStorage.SetAsync("role", "user");
                    await SecureStorage.SetAsync("fullname", result.data.data.fullname);
                    await SecureStorage.SetAsync("email", result.data.data.email);
                    await Application.Current.MainPage.Navigation.PushModalAsync(new HomePage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Notification", result.message, "OK");
                }
            }
            if(salerRadioButton.IsChecked == true)
            {
                var result = await userService.adminLogin(userName.Text, userPassword.Text);

                Console.WriteLine(JsonConvert.SerializeObject(result));
                if (result != null && result.status == 1)
                {
                    await SecureStorage.SetAsync("role", "saler");
                    await SecureStorage.SetAsync("token", result.data.token);
                    await SecureStorage.SetAsync("fullname", "Admin");

                    await Application.Current.MainPage.Navigation.PushModalAsync(new CategoryPage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Notification", result.message, "OK");
                }
            }
            
        }

        private async void RegisterClicked(object sender, EventArgs e)
        {
            if(registerPassword.Text!= registerPasswordVerify.Text)
            {
                await Application.Current.MainPage.DisplayAlert("Notification", "Your password not match", "OK");
            }
            else
            {
                UserData userData = new UserData()
                {
                    username = registerUsername.Text,
                    password = registerPassword.Text,
                    email = registerEmail.Text,
                    phone = registerPhone.Text,
                    fullname = registerFullname.Text,
                };
                var result = await userService.userRegister(userData);
                Console.WriteLine(JsonConvert.SerializeObject(result));
                if (result != null && result.status == 1)
                {
                    await SecureStorage.SetAsync("token", result.data.token);
                    await SecureStorage.SetAsync("userId", result.data.data.id.ToString());
                    //await SecureStorage.SetAsync("fullName", result.data.data.fullname);
                    await Application.Current.MainPage.Navigation.PushModalAsync(new HomePage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Notification", result.message, "OK");
                }
            }
            //Navigation.PushAsync(new RegisterPage());
            
        }
    }
}