using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Models;
using System.Runtime.CompilerServices;
using AutoCareDiray.View;
using AutoCareDiray.Models.Authorization;

namespace AutoCareDiray.ViewModels
{
    
    public partial class AuthorizationViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
        private UserResponse _userResponse;
        public AuthorizationViewModel(IApiService apiService) 
        {
            _apiService = apiService;
        }

        [ObservableProperty]
        public string login;
        [ObservableProperty]
        public string password;
        [ObservableProperty]
        public string text;
        [ObservableProperty]
        public bool isToggleSwitch;
        [ObservableProperty]
        public bool isPassword = true;
        [ObservableProperty]
        public bool isTogglePasswordSwitch;

        [RelayCommand]
        public async void LogIn()
        {
            if(Password == "1") await Shell.Current.GoToAsync("//MainPage");
            try
            {
                bool start = AuthorizationValidation.AuthValidation(Login, Password);
                if (start)
                {
                    Text = "";
                    _userResponse = await _apiService.AuthorizationApiAsync(login, password);
                    Text = _userResponse.Message;
                    if (_userResponse is not null)
                    {
                        if (_userResponse.Success == true)
                        {
                            PreferencesSetUser(_userResponse);

                            await Shell.Current.GoToAsync("//MainPage");
                        }
                        else Text = _userResponse.Message;
                    }
                }
                else Text = "Пароль и Логин не могут быть пустыми";
            }
            catch (HttpRequestException ex)
            {
                Text = "Соединение не установлено проверте подключение к интернету или сервер не доступен ";
            }
            catch (Exception ex)
            {
                Text = ex.Message;
            }
        }
        [RelayCommand]
        public async void Registration()
        {
            await Shell.Current.Navigation.PushModalAsync(new RegistrationPage(_apiService),true);
        }

        void PreferencesSetUser(UserResponse userResponse)
        {
            Preferences.Set("User_id", userResponse.User_id.ToString());
            Preferences.Set("NickName", userResponse.NickName);
            Preferences.Set("Email", userResponse.Email);
            if(IsToggleSwitch) Preferences.Set("is_login", true);
        }

        partial void OnIsTogglePasswordSwitchChanged(bool value)
        {
            IsPassword = !IsTogglePasswordSwitch;
        }
    }
}
