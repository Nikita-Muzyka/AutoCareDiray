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

        Task<bool> DeleteUserApiAsync(int User_id);
    }
}
