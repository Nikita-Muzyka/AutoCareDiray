using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AutoCareDiray.Shared.Service.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using AutoCareDiray.Shared.Interface;

namespace AutoCareDiray.Shared.ViewModels.VehicleViewModel
{
    public partial class ListVehicleViewModel : BaseViewModel
    {
        private CancellationTokenSource _cts;

        [ObservableProperty]
        private ObservableCollection<Vehicle> vehicles = new();

        [ObservableProperty]
        private string errors;
        [ObservableProperty]
        private Vehicle selectedVehicle;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="apiService"></param>
        public ListVehicleViewModel(IDialogService dialog, IDataService data, INavigationService navigate)
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public async void GoCreateVehicle()
        {
            await _navigationService.GoNavigation("CreateVehicleView");
        }
        [RelayCommand]
        public async Task GoCarCard(Vehicle VehicleSelected)
        {
            var property = new Dictionary<string, object>()
            {
                ["VehicleId"] = VehicleSelected.Id
            };
            await _navigationService.GoNavigation("CardVehicleView", property);
        }

        [RelayCommand]
        /// <summary>
        /// Загрузка авто с сервера
        /// </summary>
        public async Task LoadVehicles()
        {
            if(Vehicles.Count > 0) Vehicles.Clear();
            var cars = await _dataService.ListVehicleAsync(_cts.Token);
            if(cars != null)
            {
                foreach (var addcar in cars)
                {
                    Vehicles.Add(addcar);
                }
            }
        }

        [RelayCommand]
        public async Task DeleteVehicle(Vehicle vehicleSelected)
        {
            var result = await _dialogService.ShowConfirmationAsync(vehicleSelected.NameVehicle);
            if (result)
            {
                await _dataService.DeleteVehicleAsync(vehicleSelected, _cts.Token);
                await LoadVehicles();
            }
        }

        [RelayCommand]
        public async Task EditVehicle(Vehicle vehicleSelected)
        {
            var property = new Dictionary<string, object>()
            {
                ["VehicleId"] = vehicleSelected.Id
            };

            await _navigationService.GoNavigation("CreateVehicleView", property);
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
