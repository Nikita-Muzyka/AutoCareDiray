using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Service;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.View;

namespace AutoCareDiray.ViewModels
{
    public partial class CreateCarsViewModal : ObservableObject
    {
        private readonly IApiService _apiService;
        public CreateCarsViewModal(IApiService apiService) 
        {
            _apiService = apiService;
        }
        [RelayCommand]
        public async void UserSettingsGo()
        {
            await Shell.Current.GoToAsync("//UserSettingsPage");
        }
    }
}
