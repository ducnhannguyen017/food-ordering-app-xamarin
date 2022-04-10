using FoodOrderingApp.Services.IServices;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodOrderingApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPopup : Xamarin.CommunityToolkit.UI.Views.Popup
    {
        ICartService cartService = DependencyService.Get<ICartService>();
        IOrderService orderService = DependencyService.Get<IOrderService>();

        public PaymentPopup()
        {
            InitializeComponent();
        }

        public int success;
        private async void Pay_Clicked(object sender, EventArgs e)
        {
            StripeConfiguration.ApiKey = "sk_test_51KknIZBXB4E5SQY3bPc7F9pSRgH29ejsUDTab6XAYb17wpMuO3C97EYNLmnzOTJFBbOfBrc81bjLjS0kjIJ0mBoV00eWL8vyjo";

            string cardno = "4000000000003055";
            string expMonth = "08";
            string expYear = "2023";
            string cardCvv = "123";

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
                Currency = "inr",
                Token = newToken.Id
            };

            var sourceService = new SourceService();
            Source source = sourceService.Create(option);

            // step 4: create customer
            CustomerCreateOptions customer = new CustomerCreateOptions
            {
                Name = "SP Tutorial",
                Email = "spaltutorials@gmail.com",
                Description = "Paying 10 Rs",
                Address = new AddressOptions { City = "Kolkata", Country = "India", Line1 = "Sample Address", Line2 = "Sample Address 2", PostalCode = "700030", State = "WB" }
            };

            var customerService = new CustomerService();
            var cust = customerService.Create(customer);

            // step 5: charge option
            var chargeoption = new ChargeCreateOptions
            {
                Amount = 45000,
                Currency = "INR",
                ReceiptEmail = "spaltutorials@gmail.com",
                Customer = cust.Id,
                Source = source.Id
            };

            // step 6: charge the customer
            var chargeService = new ChargeService();
            Charge charge = chargeService.Create(chargeoption);
            if (charge.Status == "succeeded")
            {
                // success
                //Console.WriteLine("27");
                //var result = await orderService.createOrder(Id);
                //Console.WriteLine("62" + JsonConvert.SerializeObject(result));
                //if (result != null && result.status == 1)
                //{
                //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Notification", "Success", "OK");
                //    //getData();
                //}
                //else
                //{
                //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Notification", "Connection Error", "OK");
                //}
            }
            else
            {
                // failed
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Notification", "Connection Error", "OK");

            }
        }
    }
}