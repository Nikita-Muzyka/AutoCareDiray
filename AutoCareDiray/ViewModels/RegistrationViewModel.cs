using AutoCareDiray.Models;
using AutoCareDiray.Service;
using AutoCareDiray.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AutoCareDiray.ViewModels
{
    partial class RegistrationViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
        UserValidation _userValidation = new();
        AuthResponse _authResponse;

        [ObservableProperty]
        public string message;
        [ObservableProperty]
        public bool isValid = true;

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
            _userValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI();
        }

        public bool HasErrors => _userValidation.HasErrors;
        public string NickNameError => _userValidation.GetErrors("NickName") as string;
        public string EmailError => _userValidation.GetErrors("Email") as string;
        public string LoginError => _userValidation.GetErrors("Login") as string;
        public string PasswordError => _userValidation.GetErrors("Password") as string;


        [RelayCommand]
        public async void CreateUserDTO()
        {
            _userValidation.ValidationAll(NickName, Email, Login, Password);
            IsValid = !HasErrors;
            if (IsValid)
            {
                RegistrationApi();
            }
        }
        [RelayCommand]
        public async void CloseModalView()
        {
            await Shell.Current.Navigation.PopModalAsync();
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
            _userValidation.ValidationLogin(value);
        }
        partial void OnPasswordChanged(string value)
        {
            _userValidation.ValidationPassword(value);
        }

        void OnErrorsChangedUI()
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(NickNameError));
            OnPropertyChanged(nameof(EmailError));
            OnPropertyChanged(nameof(LoginError));
            OnPropertyChanged(nameof(PasswordError));
        }

        public async void RegistrationApi()
        {
            UserDTO userDTO = new UserDTO(NickName, Email, Login, Password);
            try
            {
                _authResponse = await _apiService.CreateUserApiAsync(userDTO);
                message = _authResponse.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            if (_authResponse is not null)
            {
                if (_authResponse.Success == true)
                {
                    await Shell.Current.Navigation.PopModalAsync();
                    await Shell.Current.Navigation.PushAsync(new MainPage());
                }
            }
        }
    }
}
