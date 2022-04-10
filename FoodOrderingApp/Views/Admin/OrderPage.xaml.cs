using FoodOrderingApp.Models;
using FoodOrderingApp.Services.IServices;
using Java.Lang;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodOrderingApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage, INotifyPropertyChanged
    {
        IOrderService orderService = DependencyService.Get<IOrderService>();
        public OrderPage()
        {
            InitializeComponent();
            BindingContext = this;
            GetAllOrders();
            SelectOrderCommand = new Command<CartData>((model) => ExecuteSelectOrderCommand(model));

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;

        }
        public Command SelectOrderCommand { get; }

        private ObservableCollection<OrderFood> _orders;
        public ObservableCollection<OrderFood> Orders
        {
            get { return _orders; }
            set { SetProperty(ref _orders, value); }
        }
        private ObservableCollection<CartData> _orderData;
        public ObservableCollection<CartData> OrderData
        {
            get { return _orderData; }
            set { SetProperty(ref _orderData, value); }
        }
        public async void GetAllOrders()
        {
            var result= new OrderAdmin();
            if (waitingRadioButton.IsChecked == true)
            {
                 result = await orderService.getAllOrders(1);
            }
            if(confirmedRadioButton.IsChecked == true)
            {
                 result = await orderService.getAllOrders(2);
            }
            var orders = new ObservableCollection<OrderFood>();
            if (result != null)
            {
                OrderData = result.data.data;
                //foreach (var order in result.data.data)
                //{
                //    foreach(var order_food in order.order_foods)
                //    {
                //        orders.Add(order_food);
                //    }
                //}
            }

        }

        private async void orderCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            GetAllOrders();
        }
        private void ExecuteSelectOrderCommand(CartData model)
        {
            var index = OrderData
                    .ToList()
                    .FindIndex(p => p.id == model.id);
            Orders = OrderData[index].order_foods;

            tabView.SelectedIndex = 1;
        }

        private async void ConfirmOrder(object sender, EventArgs e)
        {
            var result =await orderService.confirmOrder(Integer.ParseInt(((TappedEventArgs)e).Parameter.ToString()));
            Console.WriteLine("31" + JsonConvert.SerializeObject(result));
            if (result != null&& result.status == 1)
            {
                 GetAllOrders();
                await Application.Current.MainPage.DisplayAlert("Notification", "Success", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Notification", "Connection Error", "OK");

            }
        }
    }
}