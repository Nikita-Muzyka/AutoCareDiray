using AutoCareDiray.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Models;

namespace AutoCareDiray.ViewModels
{
    partial class RegistrationViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
        UserValidation _userValidation = new();

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
        public string LoginError => _userValidation.GetErrors("Login") as string;


        [RelayCommand]
        public async void CreateUserDTO()
        {
            
            await Shell.Current.Navigation.PopModalAsync();
            await Shell.Current.Navigation.PushAsync(new MainPage());
        }
        [RelayCommand]
        public async void CloseModalView()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        partial void OnLoginChanged(string value)
        {
            _userValidation.ValidationLogin(value);
        }

        void OnErrorsChangedUI()
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(LoginError));
        }
    }
}
