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


namespace AutoCareDiray.ViewModels
{
    
    public partial class AuthorizationViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
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
            if (Password == "1") await Shell.Current.GoToAsync("//Main/MainPage");

            bool start = AuthorizationValidation.AuthValidation(Login, Password);
            if (start)
            {
                Text = "";
                var response = await _apiService.AuthorizationApiAsync(login, password);

                if (response.Success == true)
                {
                    Text = response.Message;
                    PreferencesSetUser(response);
                    await Shell.Current.GoToAsync("//Main/MainPage");
                }
                else Text = response.Message;
            }
            else Text = "Пароль и Логин не могут быть пустыми";
        }
        [RelayCommand]
        public async void Registration()
        {
            await Shell.Current.Navigation.PushModalAsync(_registrationPage,true);
        }

        void PreferencesSetUser(ApiResponse ApiResponse)
        {
            GetUserResponse? userResponse = ApiResponse as GetUserResponse;
            Preferences.Set("User_id", userResponse.User_Id.ToString());
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
