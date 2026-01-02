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
        //User
        Task<ApiResponse> AuthorizationApiAsync(string login, string password,CancellationToken token);
        Task<ApiResponse> CreateUserApiAsync(UserDTO user,CancellationToken token);
        Task<ApiResponse> CheckUserLoginAsync(string login,CancellationToken token);
        Task<ApiResponse> UpdateUserApiAsync(UserUpdateRequest userRequest, CancellationToken token);
        Task<ApiResponse> DeleteUserApiAsync(int User_id, CancellationToken token);

        //Car
        Task<ApiResponse> CreateCarApiAsync(Car car, CancellationToken token);
        Task<ApiResponse> GetCarByUserIdApiAsync(CancellationToken token);
    }
}
