using FoodOrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingApp.Services.IServices
{
    public interface ICartService
    {
        Task<CartResponse> addToCart(int foodId, int quantity);
        Task<CartResponse> updateCart(int foodId, int quantity);
        Task<Cart> getCart();
    }
}
