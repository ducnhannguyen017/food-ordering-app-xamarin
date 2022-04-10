using FoodOrderingApp.Constant;
using FoodOrderingApp.Models;
using FoodOrderingApp.Services.IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FoodOrderingApp.Services
{
    public class OrderService:IOrderService
    {
        public async Task<OrderAdmin> getAllOrders(int status)
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            HttpResponseMessage responseMessage = await client.GetAsync(Api.BASEURL + "api/cms/list-order?status="+status);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var json = JsonConvert.DeserializeObject<OrderAdmin>(result, settings);
                return json;
            }

            return null;
        }
        public async Task<Cart> confirmOrder(int id)
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            var request = new
            {
                id=id
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/cms/confirm-order",content);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var json = JsonConvert.DeserializeObject<Cart>(result, settings);
                return json;
            }

            return null;
        }
        public async Task<Cart> createOrder(int id, string address)
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            var request = new
            {
                id=id,
                address = address
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/create-order",content);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var json = JsonConvert.DeserializeObject<Cart>(result, settings);
                return json;
            }

            return null;
        }
    }
}
