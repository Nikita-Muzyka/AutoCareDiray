using AutoCareDiray.Models;
using AutoCareDiray.Service;
using AutoCareDiray.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AutoCareDiray.ViewModels
{
    partial class RegistrationViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
        UserValidation _userValidation;
        UserResponse _userResponse;
        CancellationTokenSource _debounce;

        [ObservableProperty]
        public string text;
        
        [ObservableProperty]
        public string nickName;

        [ObservableProperty]
        public string? email;

        [ObservableProperty]
        public string login;
        
        [ObservableProperty]
        public string password;

        public RegistrationViewModel(IApiService api)
        {
            _apiService = api;
            _userValidation = new UserValidation(_apiService);
            _userValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
        }

        public bool HasErrors => _userValidation.HasErrors;
        public bool IsValidButton => !_userValidation.HasErrors;
        public string NickNameError => _userValidation.GetErrors("NickNameError") as string;
        public string EmailError => _userValidation.GetErrors("EmailError") as string;
        public string LoginError => _userValidation.GetErrors("LoginError") as string;
        public string PasswordError => _userValidation.GetErrors("PasswordError") as string;


        [RelayCommand]
        public async void CreateUserDTO()
        {
            _userValidation.ValidationAll(NickName, Email, Login, Password);
            if (HasErrors) ;
            else RegistrationApi();
        }
        [RelayCommand]
        public async void CloseModalView()
        {
            await Shell.Current.GoToAsync("..");
        }

        
        partial void OnNickNameChanged(string value)
        {
            _userValidation.ValidationNickName(value);
        }
        partial void OnEmailChanged(string value)
        {
            _userValidation.ValidationEmail(value);
        }
        partial void OnLoginChanged(string value)
        {
           DebounceSearch(value);
        }
        partial void OnPasswordChanged(string value)
        {
            _userValidation.ValidationPassword(value);
        }
        void OnErrorsChangedUI(DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(IsValidButton));
            OnPropertyChanged(e.PropertyName);
        }

        public async void RegistrationApi()
        {
            UserDTO userDTO = new UserDTO(NickName, Email, Login, Password);

            _userResponse = await _apiService.CreateUserApiAsync(userDTO);
            if (_userResponse.Success == true)
            {
                Text = _userResponse.Message;
                Thread.Sleep(1000);
                await PreferencesSetUser(_userResponse);
                await Shell.Current.Navigation.PopModalAsync();
                await Shell.Current.GoToAsync("..");
            }
            else Text = _userResponse.Message;
        }

        async Task PreferencesSetUser(UserResponse userResponse)
        {
            Preferences.Set("User_id", userResponse.User_id.ToString());
            Preferences.Set ("NickName", userResponse.NickName);
            Preferences.Set("Email", userResponse.Email);
        }

        async void DebounceSearch(string value)
        {
            try
            {
                _debounce?.Cancel();
                _debounce = new CancellationTokenSource();

                await Task.Delay(1000, _debounce.Token);
                _userValidation.ValidationLogin(value);
            }
            catch (TaskCanceledException ex)
            {

            }
        }
    }
}
