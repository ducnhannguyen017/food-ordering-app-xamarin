using FoodOrderingApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using FoodOrderingApp.Constant;
using Newtonsoft.Json;
using FoodOrderingApp.Services.IServices;
using FoodOrderingApp.Common;
using System.Text;
using Xamarin.Essentials;
using System;
using System.Net.Http.Headers;
using System.IO;
using System.Linq;

namespace FoodOrderingApp.Services
{
    public class FoodService:IFoodService
    {
        public async Task<Food> getAllFoodsAdmin()
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            responseMessage = await client.GetAsync(Api.BASEURL + "api/cms/list-food");

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<Food>(result);
                return json;
            }

            return null;
        }
        public async Task<Food> getFoodByCategory(int categoryId, int number)
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var role = await SecureStorage.GetAsync("role");
            if (role == "user")
            {
                if (categoryId > 0)
                {
                     responseMessage = await client.GetAsync(Api.BASEURL + "api/list-food?category_id=" + categoryId);
                }
                else
                {
                    Console.WriteLine("28");
                    responseMessage = await client.GetAsync(Api.BASEURL + "api/list-food");
                    Console.WriteLine(responseMessage);
                }
            }
            else
            {
                if (categoryId > 0)
                {
                    responseMessage = await client.GetAsync(Api.BASEURL + "api/cms/list-food?category_id=" + categoryId);
                }
                else
                {
                    responseMessage = await client.GetAsync(Api.BASEURL + "api/cms/list-food");
                    Console.WriteLine(responseMessage);
                }
            }
            

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<Food>(result);
                return json;
            }

            return null;
        }

        public async Task<ReponseMessage> switchFavourite(int foodId)
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            RequestFavourite request = new RequestFavourite()
            {
                food_id = foodId,
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/favourite",content);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<ReponseMessage>(result);
                return json;
            }
            return null;
        }
        public async Task<ReponseMessage> addRatingFood(int foodId, int ratingStar)
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            RequestRating request = new RequestRating()
            {
                food_id = foodId,
                value = ratingStar,
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/create-rate", content);
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<ReponseMessage>(result);
                return json;
            }
            return null;
        }

        public async Task<Food> getFoodFavourite()
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            HttpResponseMessage responseMessage = await client.GetAsync(Api.BASEURL + "api/list-favourite");

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<Food>(result);
                return json;
            }

            return null;
        }

        public async Task<FoodManipulationResponse> addFood(string name, string description, int price, FileResult file, int category_id)
        {
            try
            {
                HttpClient client = new HttpClient();
                var token = await SecureStorage.GetAsync("token");
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Headers.ContentType.MediaType = "multipart/form-data";

                var upfilebytes = File.ReadAllBytes(file.FullPath);
                content.Add(new ByteArrayContent(upfilebytes, 0, upfilebytes.Count()), "sampleFile", file.FileName);
                content.Add(new StringContent(name), "name");
                content.Add(new StringContent(description), "description");
                content.Add(new StringContent(price.ToString()), "price");
                content.Add(new StringContent(category_id.ToString()), "category_id");


                HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/cms/create-food", content);

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await responseMessage.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<FoodManipulationResponse>(result);
                    return json;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<FoodManipulationResponse> updateFood(string name, string description, int price, FileResult file, int category_id, int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                var token = await SecureStorage.GetAsync("token");
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Headers.ContentType.MediaType = "multipart/form-data";

                var upfilebytes = File.ReadAllBytes(file.FullPath);
                content.Add(new ByteArrayContent(upfilebytes, 0, upfilebytes.Count()), "sampleFile", file.FileName);
                content.Add(new StringContent(name), "name");
                content.Add(new StringContent(description), "description");
                content.Add(new StringContent(price.ToString()), "price");
                content.Add(new StringContent(category_id.ToString()), "category_id");
                content.Add(new StringContent(id.ToString()), "id");


                HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/cms/update-food", content);

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await responseMessage.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<FoodManipulationResponse>(result);
                    return json;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<FoodManipulationResponse> deleteFood(int id)
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            var request = new
            {
                id = id,
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/cms/delete-food", content);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<FoodManipulationResponse>(result);
                return json;
            }

            return null;
        }

        public class RequestFavourite
        {
            public int food_id;
        }
        public class RequestRating
        {
            public int food_id;
            public int value;
        }

    }
}
