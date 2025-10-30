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
                if (result == null) return new AuthResponse();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<AuthResponse> CreateUserApiAsync(UserDTO user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/register", user);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if(result == null) return new AuthResponse();

            return result;
        }

    }
}

