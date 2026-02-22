using AutoCareDiray.Service;
using AutoCareDiray.Service.Navigation;
using AutoCareDiray.View.VehicleView;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AutoCareDiray.Service.Data;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AutoCareDiray.ViewModels.VehicleViewModel
{
    public partial class ListVehicleViewModel : BaseViewModel
    {
        private CancellationTokenSource _cts;

        [ObservableProperty]
        private ObservableCollection<Vehicle> vehicles;

        [ObservableProperty]
        private string errors;
        [ObservableProperty]
        private Vehicle selectedVehicle;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="apiService"></param>
        public ListVehicleViewModel(IApiService apiService, IDialogService dialogService,IDataService dataService,INavigationService navigation) 
            : base(apiService,dialogService,dataService,navigation)
        {
            Vehicles = new ObservableCollection<Vehicle>();
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public async void GoCreateVehicle()
        {
            await _navigationService.GoNavigation(nameof(CreateVehicleView));
        }
        [RelayCommand]
        public async Task GoCarCard(Vehicle VehicleSelected)
        {
            var property = new Dictionary<string, object>()
            {
                ["VehicleId"] = VehicleSelected.Id
            };
            await _navigationService.GoNavigation(nameof(CardVehicleView), property);
        }

        [RelayCommand]
        /// <summary>
        /// Загрузка авто с сервера
        /// </summary>
        public async void LoadVehicles()
        {
            //try
            //{
            //    var response = await _apiService.GetCarByUserIdApiAsync(_cts.Token);
            //    var carsResponse = response as CarListResponse;

            //    if (carsResponse != null)
            //    {
            //        foreach (var addcar in carsResponse.cars)
            //        {
            //            Vehicles.Add(addcar);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Errors = ex.Message;
            //}
            Vehicles.Clear();
            foreach (var addcar in await _dataService.ListVehicleAsync(_cts.Token))
            {
                Vehicles.Add(addcar);
            }
        }

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
}
