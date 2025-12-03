using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Models;
using AutoCareDiray.Service;
using AutoCareDiray.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoCareDiray.ViewModels.Cars
{
    public partial class CarCardViewModal : ObservableObject
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        Car car;
        public CarCardViewModal(Car car, IApiService apiService)
        {
            this.car = car;
            _apiService = apiService;
        }

        [RelayCommand]
        public async void CreateMaintenanse()
        {
            await Shell.Current.Navigation.PushAsync(new CreateMaintenanse(_apiService));
        }
    }
}
