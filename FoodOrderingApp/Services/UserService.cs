using FoodOrderingApp.Constant;
using FoodOrderingApp.Models.Login;
using FoodOrderingApp.Models.Register;
using FoodOrderingApp.Services.IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingApp.Services
{
    public class UserService : IUserService
    {
        public async Task<LoginResponse> userLogin(string username, string password)
        {
            HttpClient client = new HttpClient();
            LoginRequest loginRequest = new LoginRequest()
            {
                username = username,
                password = password
            };
            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/login", content);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<LoginResponse>(result);
                return json;
            }

            return null;
        }
        public async Task<LoginResponse> adminLogin(string username, string password)
        {
            HttpClient client = new HttpClient();
            LoginRequest loginRequest = new LoginRequest()
            {
                username = username,
                password = password
            };
            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/cms/login", content);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<LoginResponse>(result);
                return json;
            }

            return null;
        }

        public async Task<RegisterResponse> userRegister(UserData userRegister)
        {
            HttpClient client = new HttpClient();
            RegisterRequest registerRequest = new RegisterRequest()
            {
                username = userRegister.username,
                password = userRegister.password,
                email = userRegister.email,
                phone = userRegister.password,
                fullname = userRegister.fullname,
            };
            var content = new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/register", content);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<RegisterResponse>(result);
                return json;
            }

            return null;
        }
    }
}
