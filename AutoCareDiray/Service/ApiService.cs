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

        public async Task<string> AuthorizationApiAsync(string login,string password)
        {
            var userAuth = new UserAuthorization
            {
                Login = login,
                Password = password
            };
            var response = await _httpClient.PostAsJsonAsync("api/User/login", userAuth);

            if (response.IsSuccessStatusCode)
            {
                // Ваш API возвращает { Message = "Успешный вход" }
                var result = await response.Content.ReadFromJsonAsync<string>();
                return result;
            }
            else
            {
                return "No";
            }
        }

    }
}

