using FoodOrderingApp.Models.Login;
using FoodOrderingApp.Models.Register;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingApp.Services.IServices
{
    public interface IUserService
    {
        Task<LoginResponse> userLogin(string username, string password);
        Task<LoginResponse> adminLogin(string username, string password);
        Task<RegisterResponse> userRegister(UserData userRegister);
    }
}
