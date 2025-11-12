using AutoCareDiray.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return result;
                }
                else return new UserResponse { Message = responseContent, Success = false };

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
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return result;
                }
                else return new UserResponse { Message = responseContent, Success = false };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // HTTP Delete  User
        public async Task<bool> DeleteUserApiAsync(int User_id)
        {
            var response = await _httpClient.DeleteAsync($"api/User/delete/{User_id}");
            if (response.IsSuccessStatusCode) return true;
            else return false;
        }

    }
}

