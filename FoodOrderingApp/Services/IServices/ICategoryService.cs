using FoodOrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FoodOrderingApp.Services.IServices
{
    public interface ICategoryService
    {
        Task<Category> getAllCategories();
        Task<CategoryResponse> updateCategory(string name, string description, FileResult file, int id);
        Task<CategoryResponse> deleteCategory(int id);
        Task<CategoryResponse> addCategory(string name, string description, FileResult file);
    }
}
