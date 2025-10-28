using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels
{
    
    public partial class AuthorizationViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
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
        public string text;

        [RelayCommand]
        public void Authorization()
        {
            var result = _apiService.AuthorizationApiAsync(login, password);
            Text = result.ToString();
        }
    }
}
