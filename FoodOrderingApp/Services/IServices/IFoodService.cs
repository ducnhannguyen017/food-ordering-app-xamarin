using FoodOrderingApp.Common;
using FoodOrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FoodOrderingApp.Services.IServices
{
    public interface IFoodService
    {
        Task<Food> getAllFoodsAdmin();
        Task<Food> getFoodByCategory(int categoryId, int number);
        Task<ReponseMessage> switchFavourite(int foodId);
        Task<ReponseMessage> addRatingFood(int foodId, int ratingStar);
        Task<Food> getFoodFavourite();
        Task<FoodManipulationResponse> addFood(string name,string description, int price, FileResult file, int category_id);
        Task<FoodManipulationResponse> updateFood(string name,string description, int price, FileResult file, int category_id, int id);
        Task<FoodManipulationResponse> deleteFood( int id);
    }
}
