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

        public async Task<AuthResponse> AuthorizationApiAsync(string login,string password)
        {
            try
            {
                var userAuth = new UserAuthorization
                {
                    Login = login,
                    Password = password
                };
                var response = await _httpClient.PostAsJsonAsync("api/User/login", userAuth);
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<AuthResponse> CreateUserApiAsync(UserDTO user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/User/register", user);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    return result;
                }
                else return new AuthResponse { Message = responseContent, Success = false };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}

