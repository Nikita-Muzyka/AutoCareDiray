using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AutoCareDiray.Shared.Service.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using AutoCareDiray.Shared.Service.ResultService;
using AutoCareDiray.Shared.Interface;
using System.Diagnostics;

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
            var result = await _dataService.ListVehicleAsync(_cts.Token);
            if (result.Success)
            {
                var resultVehicles = result as Result<List<Vehicle>>;
                var cars = resultVehicles.Data;
                CheckWarningRepair(cars);

                foreach (var addcar in cars)
                {
                    Vehicles.Add(addcar);
                }
            }
            else await _dialogService.ShowToastAsync(result.ErrorMessage);
        }

        [RelayCommand]
        public async Task DeleteVehicle(Vehicle vehicleSelected)
        {
            var respon = await _dialogService.ShowConfirmationAsync(vehicleSelected.NameVehicle);
            if (respon)
            {
                var result = await _dataService.DeleteVehicleAsync(vehicleSelected, _cts.Token);
                if(result.Success)
                {
                    await _dialogService.ShowToastAsync("ТС удалено");
                    await LoadVehicles();
                }
                else await _dialogService.ShowToastAsync(result.ErrorMessage);
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


        private void CheckWarningRepair(IEnumerable<Vehicle> cars)
        {
            foreach(var list in cars)
            {
                var repairsType = list.RepairTypes.Where(c => list.Mileage - c.LastServiceMileage > c.IntervalMileage).ToList();
                int count = repairsType.Count;
                list.WarningRepair = $"Внимание:{count}";
            }
        }
    }
}
