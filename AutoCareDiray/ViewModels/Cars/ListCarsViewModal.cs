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
        [ObservableProperty]
        public Car selectedCar;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="apiService"></param>
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
        [RelayCommand]
        public async void GoCarCard()
        {
            if (SelectedCar is null)
            {
                Errors = "Ошибка";
            }
            else await Shell.Current.Navigation.PushAsync(new CarCardPage(selectedCar,_apiService));
        }

        /// <summary>
        /// Загрузка авто с сервера
        /// </summary>
        async void LoadCars()
        {
            var car = new Car
            {
                Brand = "Chevrolet",
                Model = "Lachetti",
                Year = 2211,
                Year_purchase = 2221,
                Car_id = 5555,
                Current_mileage = 200000,
                Engine_type = "Бензин",
                Transmission_box = "Механическая",
                Vin = "dawdawdadadadaw",
                User_id = 1
            };
            Cars.Add(car);

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
