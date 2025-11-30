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
        private RegistrationPage _registrationPage;
        public AuthorizationViewModel(IApiService apiService) 
        {
            _apiService = apiService;
            _registrationPage = new RegistrationPage(apiService);
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
            if (Password == "1") await Shell.Current.GoToAsync("//Main");

            bool start = AuthorizationValidation.AuthValidation(Login, Password);
            if (start)
            {
                Text = "";
                _userResponse = await _apiService.AuthorizationApiAsync(login, password);

                if (_userResponse.Success == true)
                {
                    Text = _userResponse.Message;
                    PreferencesSetUser(_userResponse);
                    await Shell.Current.GoToAsync("//Main");
                }
                else Text = _userResponse.Message;
            }
            else Text = "Пароль и Логин не могут быть пустыми";
        }
        [RelayCommand]
        public async void Registration()
        {
            await Shell.Current.Navigation.PushModalAsync(_registrationPage,true);
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
