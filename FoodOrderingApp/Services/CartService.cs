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
    public class CartService : ICartService
    {
        public async Task<CartResponse> addToCart(int foodId, int quantity)
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            CartRequest cartRequest = new CartRequest()
            {
                food_id = foodId,
                quantity = quantity
            };
            var content = new StringContent(JsonConvert.SerializeObject(cartRequest), Encoding.UTF8,"application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/add-cart",content);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<CartResponse>(result);
                return json;
            }

            return null;
        }
        public async Task<CartResponse> updateCart(int id, int quantity)
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            CartUpdateRequest cartRequest = new CartUpdateRequest()
            {
                id = id,
                quantity = quantity
            };
            var content = new StringContent(JsonConvert.SerializeObject(cartRequest), Encoding.UTF8,"application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/update-cart",content);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<CartResponse>(result);
                return json;
            }

            return null;
        }
        public async Task<Cart> getCart()
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            HttpResponseMessage responseMessage = await client.GetAsync(Api.BASEURL + "api/get-cart");

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<Cart>(result);
                return json;
            }

            return null;
        }
    }
}
