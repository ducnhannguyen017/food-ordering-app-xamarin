using FoodOrderingApp.Models;
using FoodOrderingApp.Services.IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingApp.Services
{
    public class PaymentService: IPaymentService
    {
        //public async Task<PaymentResponse> sendPaymentRequest(string endpoint, string postJsonString)
        //{

        //    //try
        //    //{
        //        //HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

        //        //var postData = postJsonString;

        //        //var data = Encoding.UTF8.GetBytes(postData);

        //        //httpWReq.ProtocolVersion = HttpVersion.Version11;
        //        //httpWReq.Method = "POST";
        //        //httpWReq.ContentType = "application/json";

        //        //httpWReq.ContentLength = data.Length;
        //        //httpWReq.ReadWriteTimeout = 30000;
        //        //httpWReq.Timeout = 15000;
        //        //Stream stream = httpWReq.GetRequestStream();
        //        //stream.Write(data, 0, data.Length);
        //        //stream.Close();

        //        //HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

        //        //string jsonresponse = "";

        //        //using (var reader = new StreamReader(response.GetResponseStream()))
        //        //{

        //        //    string temp = null;
        //        //    while ((temp = reader.ReadLine()) != null)
        //        //    {
        //        //        jsonresponse += temp;
        //        //    }
        //        //}


        //        ////todo parse it
        //        //return jsonresponse;
        //        ////return new MomoResponse(mtid, jsonresponse);
        //        HttpClient client = new HttpClient();
        //        var content = new StringContent(JsonConvert.SerializeObject(postJsonString), Encoding.UTF8, "application/json");
        //        HttpResponseMessage responseMessage = await client.PostAsync(endpoint,content);

        //        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
        //        {
        //            var result = await responseMessage.Content.ReadAsStringAsync();
        //            Console.WriteLine("62+"+result);
        //            var json = JsonConvert.DeserializeObject<PaymentResponse>(result);
        //            return json;
        //        }

        //        return null;

        //    //}
        //    //catch (WebException e)
        //    //{
        //    //    return e.Message;
        //    //}
        //}

        public string sendPaymentRequest(string endpoint, string postJsonString)
        {

            try
            {
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

                var postData = postJsonString;

                var data = Encoding.UTF8.GetBytes(postData);

                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";

                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;
                Stream stream = httpWReq.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                string jsonresponse = "";

                using (var reader = new StreamReader(response.GetResponseStream()))
                {

                    string temp = null;
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonresponse += temp;
                    }
                }


                //todo parse it
                return jsonresponse;
                //return new MomoResponse(mtid, jsonresponse);

            }
            catch (WebException e)
            {
                return e.Message;
            }
        }
    }
}
