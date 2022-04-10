using FoodOrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FoodOrderingApp.Services;
using FoodOrderingApp.Constant;
using Newtonsoft.Json;
using FoodOrderingApp.Services.IServices;
using Xamarin.Essentials;
using System.IO;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Linq;

namespace FoodOrderingApp.Services
{
    public class CategoryService : ICategoryService
    {
        public async Task<Category> getAllCategories()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage = await client.GetAsync(Api.BASEURL+ "api/get-category");

            if(responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<Category>(result);
                return json;
            }

            return null;
        }
        public async Task<CategoryResponse> deleteCategory(int id)
        {
            HttpClient client = new HttpClient();
            var token = await SecureStorage.GetAsync("token");
            client.DefaultRequestHeaders.Add("token", token);
            var request = new
            {
                id = id,
            };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL+ "api/cms/delete-category",content);

            if(responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<CategoryResponse>(result);
                return json;
            }

            return null;
        }
        public async Task<CategoryResponse> addCategory(string name, string description, FileResult file)
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


                HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/cms/create-category", content);

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await responseMessage.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<CategoryResponse>(result);
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
        public async Task<CategoryResponse> updateCategory(string name, string description, FileResult file, int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                var token = await SecureStorage.GetAsync("token");
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Headers.ContentType.MediaType = "multipart/form-data";
                if(file != null)
                {
                    var upfilebytes = File.ReadAllBytes(file.FullPath);
                    content.Add(new ByteArrayContent(upfilebytes, 0, upfilebytes.Count()), "sampleFile", file.FileName);
                }
                
                content.Add(new StringContent(name), "name");
                content.Add(new StringContent(description), "description");
                content.Add(new StringContent(id.ToString()), "id");

                HttpResponseMessage responseMessage = await client.PostAsync(Api.BASEURL + "api/cms/update-category", content);

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await responseMessage.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<CategoryResponse>(result);
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
    }       
}
