using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodOrderingApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderAdminPage : StackLayout
    {
        public HeaderAdminPage()
        {
            InitializeComponent();
            OpenSidebarMenuCommand = new Command(ExecuteOpenSidebarMenuCommand);
            BindingContext = this;
        }
        public Command OpenSidebarMenuCommand { get; }
        private void ExecuteOpenSidebarMenuCommand()
        {
            Application.Current.MainPage.Navigation.ShowPopup(new MenuPopup());
        }
    }
}