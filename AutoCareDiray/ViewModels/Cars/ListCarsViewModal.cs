using AutoCareDiray.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.View;
using AutoCareDiray.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AutoCareDiray.ViewModels.Cars
{
    public partial class ListCarsViewModal : ObservableObject
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        public ObservableCollection<Car> cars;
        [ObservableProperty]
        public string errors;
        public ListCarsViewModal(IApiService apiService)
        {
            _apiService = apiService;
            Cars = new ObservableCollection<Car>();
            LoadCars();
        }

        [RelayCommand]
        public async void GoCreateCar()
        {
            await Shell.Current.Navigation.PushAsync(new CreateCarsPage(_apiService));
        }
        async void LoadCars()
        {
            try
            {
                var cars = await _apiService.GetCarByUserIdApiAsync();

                foreach (var carCollection in cars)
                {
                    Cars.Add(carCollection);
                }
            }
            catch (Exception ex)
            {
                errors = ex.Message;
            }
        }
    }
}
