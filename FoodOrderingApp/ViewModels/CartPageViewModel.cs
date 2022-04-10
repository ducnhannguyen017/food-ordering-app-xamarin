using Android.Widget;
using FoodOrderingApp.Models;
using FoodOrderingApp.Services.IServices;
using FoodOrderingApp.utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nhom7_FoodOrderingApp.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodOrderingApp.ViewModels
{
    public class CartPageViewModel:BaseViewModel
    {
        ICartService cartService = DependencyService.Get<ICartService>();
        IOrderService orderService = DependencyService.Get<IOrderService>();
        IPaymentService PaymentRequest = DependencyService.Get<IPaymentService>();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CartPageViewModel()
        {
            IncreaseQuantCommand = new Command<OrderFood>(async(model)=> await ExecuteIncreaseQuantCommand(model));
            DecreaseQuantCommand = new Command<OrderFood>(async (model) => await ExecuteDecreaseQuantCommand(model));
            CreateOrderCommand = new Command(ExecuteCreateOrderCommand);
            getData();
        }
        public Command IncreaseQuantCommand { get; }
        public Command DecreaseQuantCommand { get; }
        public Command CreateOrderCommand { get; }

        private async void getData()
        {
            var result = await cartService.getCart();
            Console.WriteLine("116" + JsonConvert.SerializeObject(result));
            if (result != null&&result.status==1)
            {
                var totalPrice = 0;
                var totalQuantity = 0;
                if (result.data.order_foods != null)
                {
                    foreach (var items in result.data.order_foods)
                    {
                        totalQuantity += items.quantity;
                        totalPrice += items.price;
                    }
                }
                
                
                OrderFood = result.data.order_foods;
                TotalPrice = totalPrice;
                TotalQuantity = totalQuantity;
                Id=result.data.id;
            }
            else
            {
                OrderFood = null;
                TotalPrice = 0;
                TotalQuantity = 0;
            }
        }
        private async Task ExecuteDecreaseQuantCommand(OrderFood model)
        {
            Console.WriteLine("27");
            await cartService.updateCart(model.id, model.quantity-1);
            getData();
        }
        private async void ExecuteCreateOrderCommand()
        {
            if (TotalPrice == 0)
            {
                Toast.MakeText(Android.App.Application.Context, "Your card is emplty", ToastLength.Long).Show();
            }
            else
            {
                StripeConfiguration.ApiKey = "sk_test_51KknIZBXB4E5SQY3bPc7F9pSRgH29ejsUDTab6XAYb17wpMuO3C97EYNLmnzOTJFBbOfBrc81bjLjS0kjIJ0mBoV00eWL8vyjo";

                string cardno = CardNumber;
                string expMonth = ExpMonth;
                string expYear = ExpYear;
                string cardCvv = CardCVV;

                // Step 1: create card option

                TokenCardOptions stripeOption = new TokenCardOptions();
                stripeOption.Number = cardno;
                stripeOption.ExpYear = Convert.ToInt64(expYear);
                stripeOption.ExpMonth = Convert.ToInt64(expMonth);
                stripeOption.Cvc = cardCvv;

                // step 2: Assign card to token object
                TokenCreateOptions stripeCard = new TokenCreateOptions();
                stripeCard.Card = stripeOption;

                TokenService service = new TokenService();
                Token newToken = service.Create(stripeCard);

                // step 3: assign the token to the source
                var option = new SourceCreateOptions
                {
                    Type = SourceType.Card,
                    Currency = "vnd",
                    Token = newToken.Id
                };

                var sourceService = new SourceService();
                Source source = sourceService.Create(option);

                // step 4: create customer
                CustomerCreateOptions customer = new CustomerCreateOptions
                {
                    Name = await SecureStorage.GetAsync("fullname"),
                    Email = await SecureStorage.GetAsync("email"),
                    Description = "Paying " + TotalPrice + " Rs",
                    Address = new AddressOptions { City = "Kolkata", Country = "India", Line1 = "Sample Address", Line2 = "Sample Address 2", PostalCode = "700030", State = "WB" }
                };

                var customerService = new CustomerService();
                var cust = customerService.Create(customer);

                // step 5: charge option
                var chargeoption = new ChargeCreateOptions
                {
                    Amount = TotalPrice,
                    Currency = "VND",
                    ReceiptEmail = await SecureStorage.GetAsync("email"),
                    Customer = cust.Id,
                    Source = source.Id
                };

                // step 6: charge the customer
                var chargeService = new ChargeService();
                Charge charge = chargeService.Create(chargeoption);
                if (charge.Status == "succeeded")
                {
                    // success
                    Console.WriteLine("27");
                    var result = await orderService.createOrder(Id, Address);
                    Console.WriteLine("62" + JsonConvert.SerializeObject(result));
                    if (result != null && result.status == 1)
                    {
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Notification", "Success", "OK");
                        getData();
                    }
                    else
                    {
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Notification", "Connection Error", "OK");
                    }
                    Address = "";
                    CardNumber = "";
                    ExpYear = "";
                    ExpMonth = "";
                    CardCVV = "";
                }
                else
                {
                    // failed
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Notification", "Connection Error", "OK");

                }
            }
            
        }
        private async Task ExecuteIncreaseQuantCommand(OrderFood model)
        {
            Console.WriteLine("55" + JsonConvert.SerializeObject(model));
            var r=await cartService.updateCart(model.id, model.quantity + 1);
            Console.WriteLine("55" + JsonConvert.SerializeObject(r));

            getData();
        }
        private ObservableCollection<OrderFood> _orderFoods;
        public ObservableCollection<OrderFood> OrderFood
        {
            get { return _orderFoods; }
            set { SetProperty(ref _orderFoods, value); }
        }
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        private int _totalPrice;
        public int TotalPrice
        {
            get { return _totalPrice; }
            set { SetProperty(ref _totalPrice, value); }
        }
        private int _totalQuantity;
        public int TotalQuantity
        {
            get { return _totalQuantity; }
            set { SetProperty(ref _totalQuantity, value); }
        }
        private string _cardNumber;
        public string CardNumber
        {
            get { return _cardNumber; }
            set { SetProperty(ref _cardNumber, value); }
        }
        private string _expMonth;
        public string ExpMonth
        {
            get { return _expMonth; }
            set { SetProperty(ref _expMonth, value); }
        }
        private string _expYear;
        public string ExpYear
        {
            get { return _expYear; }
            set { SetProperty(ref _expYear, value); }
        }
        private string _cardCVV;
        public string CardCVV
        {
            get { return _cardCVV; }
            set { SetProperty(ref _cardCVV, value); }
        }
        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }
    }
}
