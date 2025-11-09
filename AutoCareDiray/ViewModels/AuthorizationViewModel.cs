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
        private AuthResponse _authResponse;
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
                _authResponse = await _apiService.AuthorizationApiAsync(login, password);
                Text = _authResponse.Message;
            }
            catch (Exception ex)
            {
                Text = ex.Message;
            }
            if ( _authResponse is not null)
            {
                if (_authResponse.Success == true)
                {
                    PreferencesSetUser(_authResponse);
                    await Shell.Current.Navigation.PopModalAsync();
                    await Shell.Current.GoToAsync("//CreateCarsPage");
                }
                else Text = _authResponse.Message;
            }
        }
        [RelayCommand]
        public async void Registration()
        {
            await Shell.Current.Navigation.PushModalAsync(new RegistrationPage(_apiService));
        }

        void PreferencesSetUser(AuthResponse authResponse)
        {
            Preferences.Set("NickName", authResponse.NickName);
            Preferences.Set("Email", authResponse.Email);
        }
    }
}
