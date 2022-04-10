using FoodOrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingApp.Services.IServices
{
    public interface IOrderService
    {
        Task<OrderAdmin> getAllOrders(int status);
        Task<Cart> confirmOrder(int id);
        Task<Cart> createOrder(int id, string address);
    }
}
