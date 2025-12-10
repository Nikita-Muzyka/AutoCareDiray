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
        Task<ApiResponse> AuthorizationApiAsync(string login, string password);
        Task<ApiResponse> CreateUserApiAsync(UserDTO user);
        Task<ApiResponse> CheckUserLoginAsync(string login);
        Task<ApiResponse> UpdateUserApiAsync(UserUpdateRequest userRequest);
        Task<ApiResponse> DeleteUserApiAsync(int User_id);
        Task<ApiResponse> CreateCarApiAsync(Car car);
        Task<ApiResponse> GetCarByUserIdApiAsync();
    }
}
