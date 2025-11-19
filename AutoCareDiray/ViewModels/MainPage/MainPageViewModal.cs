using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Service;
using AutoCareDiray.View;

namespace AutoCareDiray.ViewModels
{
    public partial class MainPageViewModal
    {
        private readonly IApiService _apiService;
        public MainPageViewModal(IApiService apiService)
        {
            _apiService = apiService;
        }
        [RelayCommand]
        public async void UserSettingsGo()
        {
            await Shell.Current.Navigation.PushAsync(new UserSettingsPage(_apiService));
        }
    }
}
