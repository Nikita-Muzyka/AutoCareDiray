using AutoCareDiray.Models;
using AutoCareDiray.Services;
using AutoCareDiray.Shared.DTOs.UserDTO;
using AutoCareDiray.Shared.Result;
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
        public async Task<Result> AuthorizationApiAsync(string login,string password, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var userAuth = new UserAuthorization(login, password);
                var response = await _httpClient.PostAsJsonAsync("api/User/login",userAuth,token);

                token.ThrowIfCancellationRequested();
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Result<UserDTO>>();
                    return result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = await response.Content.ReadFromJsonAsync<Result>();
                    return result;
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Result.ErrorCreate("Ошибка авторизации: " + result);
                }

            }
            catch(OperationCanceledException)
            {
                throw;
            }
            catch (HttpRequestException ex)
            {
                return Result.ErrorCreate("Отсутствует подключение к серверу");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate("При авторизации произошла ошибка приложения");
            }
        }

        // HTTP POST Create User
        public async Task<Result> CreateUserApiAsync(UserDTO user, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var response = await _httpClient.PostAsJsonAsync("api/User/register", user,token);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Result<UserDTO>>();
                    return result;
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Result.ErrorCreate("Ошибка создания пользователя: " + result);
                }
            }
            catch (OperationCanceledException) { throw; }
            catch (HttpRequestException ex)
            {
                return Result.ErrorCreate("Отсутствует подключение к серверу");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate("При создании пользователя произошла ошибка приложения");
            }
        }

        // HTTP POST Check Login
        public async Task<Result> CheckUserLoginAsync(UserLoginRequest userLogin, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var response = await _httpClient.PostAsJsonAsync("api/User/checkLogin", userLogin);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Result>();
                    return result;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var result = await response.Content.ReadFromJsonAsync<Result>();
                    return result;
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Result.ErrorCreate("Ошибка при проверке логина: " + result);
                }
            }
            catch (OperationCanceledException) { throw; }
            catch (HttpRequestException ex)
            {
                return Result.ErrorCreate("Отсутствует подключение к серверу");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate("При проверке логина произошла ошибка приложения");
            }
        }

        // PUT UPdate User
        public async Task<Result> UpdateUserApiAsync(int user_id,UserDTO userDto, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var response = await _httpClient.PutAsJsonAsync($"api/User/{user_id}", userDto);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Result>();
                    return result;
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Result.ErrorCreate("Ошибка при обновлении данных пользователя: " + result);
                }
            }
            catch (OperationCanceledException) { throw; }
            catch (HttpRequestException ex)
            {
                return Result.ErrorCreate("Отсутствует подключение к серверу");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate("При проверке обновлении пользователя произошла ошибка приложения");
            }
        }

        // POST UPdate Password for User
        public async Task<Result> UpdatePasswordApiAsync(int user_id, UpdatePassword updatePassword, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var response = await _httpClient.PostAsJsonAsync($"api/User/updatePassword/{user_id}", updatePassword);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Result>();
                    return result;
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Result.ErrorCreate("Ошибка при обновлении пароля пользователя: " + result);
                }
            }
            catch (OperationCanceledException) { throw; }
            catch (HttpRequestException ex)
            {
                return Result.ErrorCreate("Отсутствует подключение к серверу");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate("При создании пользователя произошла ошибка приложения");
            }
        }

        // POST recover Password for User
        public async Task<Result> RecoverPasswordApiAsync(string login, UpdatePassword updatePassword, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var response = await _httpClient.PostAsJsonAsync($"api/User/recoverPassword/{login}", updatePassword);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Result>();
                    return result;
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Result.ErrorCreate("Ошибка при восстановлении пароля пользователя: " + result);
                }
            }
            catch (OperationCanceledException) { throw; }
            catch (HttpRequestException ex)
            {
                return Result.ErrorCreate("Отсутствует подключение к серверу");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate("При восстановления пароля пользователя произошла ошибка приложения");
            }
        }

        // HTTP Delete  User
        public async Task<Result> DeleteUserApiAsync(int user_id, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var response = await _httpClient.DeleteAsync($"api/User/{user_id}");
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    var result = Result.SuccessCreate();
                    return result;
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Result.ErrorCreate("Ошибка при удалении пользователя: " + result);
                }
            }
            catch (OperationCanceledException) { throw; }
            catch (HttpRequestException ex)
            {
                return Result.ErrorCreate("Отсутствует подключение к серверу");
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate("При удалении пользователя произошла ошибка приложения");
            }
        }


        // HTTP Create Car
        public async Task<ApiResponse> CreateCarApiAsync(Car car, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var response = await _httpClient.PostAsJsonAsync($"api/Car/create", car);
                var result = await response.Content.ReadFromJsonAsync<GetCarResponse>();

                return result;
            }
            catch (OperationCanceledException) { throw; }
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
        public async Task<ApiResponse> GetCarByUserIdApiAsync(CancellationToken token)
        {
            try
            {
                var user_id = Preferences.Get("User_id", 0);
                var response = await _httpClient.GetAsync($"api/Car/getCars/{user_id}");
                token.ThrowIfCancellationRequested();

                var result = await response.Content.ReadFromJsonAsync<CarListResponse>();
                token.ThrowIfCancellationRequested();
                return result;
            }
            catch (OperationCanceledException) { throw; }
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

