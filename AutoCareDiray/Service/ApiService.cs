using AutoCareDiray.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
{
    public class ApiService : IApiService
    {
        HttpClient _httpClient;
        public ApiService(HttpClient http) 
        {
            _httpClient = http;
        }

        //HTTP POST Authoriztion
        public async Task<UserResponse> AuthorizationApiAsync(string login,string password)
        {
            try
            {
                var userAuth = new UserAuthorization
                {
                    Login = login,
                    Password = password
                };
                var response = await _httpClient.PostAsJsonAsync("api/User/login", userAuth);
                var result = await response.Content.ReadFromJsonAsync<UserResponse>();
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // HTTP POST Create User
        public async Task<UserResponse> CreateUserApiAsync(UserDTO user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/User/register", user);
                var result = await response.Content.ReadFromJsonAsync<UserResponse>();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // HTTP POST Create User
        public async Task<UserResponse> CheckUserLoginAsync(string login)
        {
            try
            {
                var user = new UserDTO
                {
                    Login = login
                };
                var response = await _httpClient.PostAsJsonAsync("api/User/checkLogin", user);
                var result = await response.Content.ReadFromJsonAsync<UserResponse>();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // HTTP Delete  User
        public async Task<UserResponse> DeleteUserApiAsync(int User_id)
        {
            var response = await _httpClient.DeleteAsync($"api/User/delete/{User_id}");
            var userResponse = new UserResponse
            {
                Message = "Пользователь удален",
                Success = true,
            };
            if (response.IsSuccessStatusCode) return userResponse;
            else
            {
                userResponse.Success = false;
                userResponse.Message = "Пользователь не найден";
                return userResponse;
            }
        }


        // HTTP Create Car
        public async Task<CarResponse> CreateCarApiAsync(Car car)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Car/create", car);
            var result = await response.Content.ReadFromJsonAsync<CarResponse>();

            return result;
        }

        // HTTP Get Cars
        public async Task<List<Car>> GetCarByUserIdApiAsync()
        {
            var user_id = Preferences.Get("User_id", 0);
            var response = await _httpClient.GetAsync($"api/Car/get/{user_id}");

            var result = await response.Content.ReadFromJsonAsync<List<Car>>();

            return result;
        }

    }
}

