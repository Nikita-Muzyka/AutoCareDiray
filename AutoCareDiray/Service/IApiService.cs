using AutoCareDiray.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
{
    public interface IApiService
    {
        Task<UserResponse> AuthorizationApiAsync(string login, string password);
        Task<UserResponse> CreateUserApiAsync(UserDTO user);
        Task<UserResponse> CheckUserLoginAsync(string login);
        Task<UserResponse> DeleteUserApiAsync(int User_id);
        Task<CarResponse> CreateCarApiAsync(Car car);
        Task<List<Car>> GetCarByUserIdApiAsync();
    }
}
