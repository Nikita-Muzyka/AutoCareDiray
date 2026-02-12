using AutoCareDiray.Models;
using AutoCareDiray.Shared.Result;
using AutoCareDiray.Shared.DTOs.UserDTO;
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
        Task<Result> AuthorizationApiAsync(string login, string password,CancellationToken token);
        Task<Result> CreateUserApiAsync(UserDTO user,CancellationToken token);
        Task<Result> CheckUserLoginAsync(UserLoginRequest userLogin,CancellationToken token);
        Task<Result> UpdateUserApiAsync(int user_id,UserDTO user, CancellationToken token);
        Task<Result> UpdatePasswordApiAsync(int user_id, UpdatePassword updatePassword, CancellationToken token);
        Task<Result> RecoverPasswordApiAsync(string login, UpdatePassword updatePassword, CancellationToken token);
        Task<Result> DeleteUserApiAsync(int User_id, CancellationToken token);
    }
}
