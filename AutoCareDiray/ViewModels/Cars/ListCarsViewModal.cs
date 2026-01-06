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
using AutoCareDiray.Services;

namespace AutoCareDiray.ViewModels.Cars
{
    public partial class ListCarsViewModal : BaseViewModel
    {
        private CancellationTokenSource _cts;

        [ObservableProperty]
        private ObservableCollection<Car> cars;

        [ObservableProperty]
        private string errors;
        [ObservableProperty]
        private Car selectedCar;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="apiService"></param>
        public ListCarsViewModal(IApiService apiService, IDialogService dialogService) : base(apiService,dialogService)
        {
            Cars = new ObservableCollection<Car>();
            _cts = new CancellationTokenSource();
#if DEBUG
            CreateCar();
#endif
        }

        [RelayCommand]
        public async void GoCreateCar()
        {
            await Shell.Current.GoToAsync(nameof(CreateCarsPage));
        }
        [RelayCommand]
        public async void GoCarCard()
        {
            if (SelectedCar is null)
            {
                Errors = "Ошибка";
            }
            else 
            {
                var property = new Dictionary<string, object>()
                {
                    ["SelCar"] = SelectedCar
                };
                await Shell.Current.GoToAsync(nameof(CarCardPage),property);
            } 
        }

        [RelayCommand]
        /// <summary>
        /// Загрузка авто с сервера
        /// </summary>
        public async void LoadCars()
        {

            try
            {
                _cts = new CancellationTokenSource();
                var response = await _apiService.GetCarByUserIdApiAsync(_cts.Token);
                var carsResponse = response as CarListResponse;

                if(carsResponse != null)
                {
                    foreach (var addcar in carsResponse.cars)
                    {
                        Cars.Add(addcar);
                    }
                }
            }
            catch (Exception ex)
            {
                Errors = ex.Message;
            }
        }
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
        }


        void CreateCar()
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
        }
    }
}
