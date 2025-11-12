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
        Task<AuthResponse> AuthorizationApiAsync(string login, string password);
        Task<AuthResponse> CreateUserApiAsync(UserDTO user);

        Task DeleteUserApiAsync(UserDTO user);
    }
}
