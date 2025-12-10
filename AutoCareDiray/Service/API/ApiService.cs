using AutoCareDiray.Models;
using AutoCareDiray.Service.APIResponse.UserResponse;
using AutoCareDiray.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
        public async Task<ApiResponse> AuthorizationApiAsync(string login,string password)
        {
            try
            {
                var userAuth = new UserAuthorization(login, password);
                var response = await _httpClient.PostAsJsonAsync("api/User/login",userAuth);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadFromJsonAsync<GetUserResponse>();
                    return result;
                }
                else
                {
                    var result = await response.Content.ReadFromJsonAsync<ErrorsResponse>();
                    return result;
                }

            }
            catch (HttpRequestException ex)
            {
                return new ErrorsResponse("Отсутствует подключение к серверу", ex.Message);
            }
            catch (Exception ex)
            {
                return new ErrorsResponse("При авторизации произошла ошибка приложения", ex.Message);
            }
        }

        // HTTP POST Create User
        public async Task<ApiResponse> CreateUserApiAsync(UserDTO user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/User/register", user);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadFromJsonAsync<GetUserResponse>();
                    return result;
                }
                else
                {
                    var result = await response.Content.ReadFromJsonAsync<ErrorsResponse>();
                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                return new ErrorsResponse("Отсутствует подключение к серверу", ex.Message);
            }
            catch (Exception ex)
            {
                return new ErrorsResponse("При создании пользователя произошла ошибка приложения", ex.Message);
            }
        }

        // HTTP POST Check LOgin
        public async Task<ApiResponse> CheckUserLoginAsync(string login)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync("api/User/checkLogin", new UserLoginRequest(login));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadFromJsonAsync<OperationResultResponse>();
                    return result;
                }
                else
                {
                    var result = await response.Content.ReadFromJsonAsync<ErrorsResponse>();
                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                return new ErrorsResponse("Отсутствует подключение к серверу", ex.Message);
            }
            catch (Exception ex)
            {
                return new ErrorsResponse("При проверке логина произошла ошибка приложения", ex.Message);
            }
        }

        // PUT UPdate User
        public async Task<ApiResponse> UpdateUserApiAsync(UserUpdateRequest userRequest)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/User/updateUser", userRequest);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadFromJsonAsync<OperationResultResponse>();
                    return result;
                }
                else
                {
                    var result = await response.Content.ReadFromJsonAsync<ErrorsResponse>();
                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                return new ErrorsResponse("Отсутствует подключение к серверу", ex.Message);
            }
            catch (Exception ex)
            {
                return new ErrorsResponse("При создании пользователя произошла ошибка приложения", ex.Message);
            }
        }

        // HTTP Delete  User
        public async Task<ApiResponse> DeleteUserApiAsync(int User_id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/User/delete/{User_id}");
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    var result = await response.Content.ReadFromJsonAsync<OperationResultResponse>();
                    return result;
                }
                else
                {
                    var result = await response.Content.ReadFromJsonAsync<ErrorsResponse>();
                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                return new ErrorsResponse("Отсутствует подключение к серверу", ex.Message);
            }
            catch (Exception ex)
            {
                return new ErrorsResponse("При удалении пользователя произошла ошибка приложения", ex.Message);
            }
        }


        // HTTP Create Car
        public async Task<ApiResponse> CreateCarApiAsync(Car car)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/Car/create", car);
                var result = await response.Content.ReadFromJsonAsync<GetCarResponse>();

                return result;
            }
            catch (HttpRequestException ex)
            {
                return new ErrorsResponse("Отсутствует подключение к серверу", ex.Message);
            }
            catch (Exception ex)
            {
                return new ErrorsResponse("При создании авто произошла ошибка приложения", ex.Message);
            }
        }

        // HTTP Get Cars
        public async Task<ApiResponse> GetCarByUserIdApiAsync()
        {
            try
            {
                var user_id = Preferences.Get("User_id", 0);
                var response = await _httpClient.GetAsync($"api/Car/getCars/{user_id}");

                var result = await response.Content.ReadFromJsonAsync<CarListResponse>();

                return result;
            }
            catch (HttpRequestException ex)
            {
                return new ErrorsResponse("Отсутствует подключение к серверу", ex.Message);
            }
            catch (Exception ex)
            {
                return new ErrorsResponse("При поиске авто пользователя произошла ошибка приложения", ex.Message);
            }
        }

    }
}

