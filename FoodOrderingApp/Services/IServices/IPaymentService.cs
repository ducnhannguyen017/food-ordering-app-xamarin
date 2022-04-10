using FoodOrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingApp.Services.IServices
{
    public interface IPaymentService
    {
        //Task<PaymentResponse> sendPaymentRequest(string endpoint, string postJsonString);
        string sendPaymentRequest(string endpoint, string postJsonString);
    }
}
