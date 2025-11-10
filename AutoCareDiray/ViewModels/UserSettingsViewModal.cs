using AutoCareDiray.Service;
using AutoCareDiray.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels
{
    public partial class UserSettingsViewModal : ObservableObject
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        public string nickName;
        [ObservableProperty]
        public string email;

        public UserSettingsViewModal(IApiService apiService) 
        {
            _apiService = apiService;
            NickName = Preferences.Get("NickName","null");
            Email = Preferences.Get("Email", "null");
        }


        [RelayCommand]
        public async void ExitProfil()
        {
            Preferences.Remove("NickName");
            Preferences.Remove("Email");

            var authPage = Application.Current.Handler.MauiContext.Services.GetService<AuthorizationPage>();
            await Shell.Current.Navigation.PushModalAsync(authPage);

        }
    }
}
