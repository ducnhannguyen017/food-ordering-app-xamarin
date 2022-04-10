using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin;

namespace FoodOrderingApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingPopup : Xamarin.CommunityToolkit.UI.Views.Popup
    {
        public RatingPopup()
        {
            InitializeComponent();
        }
        public int getRatingStar()
        {
            var a = Rating.SelectedStarValue;
            return a;   
        }
    }
}