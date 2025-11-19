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
        private UserResponse _userResponse;
        public AuthorizationViewModel(IApiService apiService) 
        {
            _apiService = apiService;
            Text = "Жду";
        }

        [ObservableProperty]
        public string login;
        [ObservableProperty]
        public string password;
        [ObservableProperty]
        public string text; // информация о результате

        [RelayCommand]
        public async void LogIn()
        {
            try
            {
                Text = "Начал авторизацию";
                _userResponse = await _apiService.AuthorizationApiAsync(login, password);
                Text = _userResponse.Message;
            }
            catch (Exception ex)
            {
                Text = ex.Message;
            }
            if (_userResponse is not null)
            {
                if (_userResponse.Success == true)
                {
                    PreferencesSetUser(_userResponse);
                    await Shell.Current.Navigation.PopModalAsync();
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else Text = _userResponse.Message;
            }
        }
        [RelayCommand]
        public async void Registration()
        {
            await Shell.Current.Navigation.PushModalAsync(new RegistrationPage(_apiService));
        }

        void PreferencesSetUser(UserResponse userResponse)
        {
            Preferences.Set("User_id", userResponse.User_id.ToString());
            Preferences.Set("NickName", userResponse.NickName);
            Preferences.Set("Email", userResponse.Email);
            Preferences.Set("is_login", true);
        }
    }
}
