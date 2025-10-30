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
using AutoCareDiray.Models.User;

namespace AutoCareDiray.ViewModels
{
    partial class RegistrationViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
        public RegistrationViewModel(IApiService api) 
        {
            _apiService = api;
        }

        [ObservableProperty]
        public string nickName;
        [ObservableProperty]
        public string email;
        [ObservableProperty]
        public string login;
        [ObservableProperty]
        public string password;

        [ObservableProperty]
        public string text;

        [RelayCommand]
        public async void CreateUserDTO()
        {
            var userDTO = UserValidation.Validation(NickName, Email, Login, Password);
            if (userDTO is null) text = "Не удачно";
            else
            {
                var response = await _apiService.CreateUserApiAsync(userDTO);
            }

            await Shell.Current.Navigation.PopModalAsync();
            await Shell.Current.Navigation.PushAsync(new MainPage());
        }
        [RelayCommand]
        public async void CloseModalView()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
