using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AutoCareDiray.Shared.Service.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using AutoCareDiray.Shared.Service.ResultService;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using System.Diagnostics;
using AutoCareDiray.Shared.Service.IntervalCalculator;

namespace AutoCareDiray.Shared.ViewModels.VehicleViewModel
{
    public partial class ListVehicleViewModel : BaseViewModel
    {
        #region основыне классы

        private CancellationTokenSource _cts;
        IPreferencesService _preferencesService;

        #endregion

        #region основыне классы для работş UI

        [ObservableProperty]
        private ObservableCollection<Vehicle> vehicles = new();

        [ObservableProperty]
        private string errors;
        [ObservableProperty]
        private Vehicle selectedVehicle;
        [ObservableProperty]
        private bool isWarningRepair = false;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="apiService"></param>
        public ListVehicleViewModel(IDialogService dialog, IDataService data, INavigationService navigate,IPreferencesService prefer)
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
            _preferencesService = prefer;
        }

        [RelayCommand]
        public async void GoCreateVehicle()
        {
            await _navigationService.GoNavigation("CreateVehicleView");
        } //навигация создания авто

        [RelayCommand]
        public async Task GoCarCard(Vehicle VehicleSelected)
        {
            var property = new Dictionary<string, object>()
            {
                ["VehicleId"] = VehicleSelected.Id
            };
            await _navigationService.GoNavigation("CardVehicleView", property);
        } //навигация карточки авто

        [RelayCommand]
        public async Task LoadVehicles()
        {
            if(Vehicles.Count > 0) Vehicles.Clear();
            var result = await _dataService.GetListVehicleAsync(_cts.Token);
            if (result.Success)
            {
                var resultVehicles = result as Result<List<Vehicle>>;
                var cars = resultVehicles.Data;
                CheckWarningRepair(cars);

                foreach (var addcar in cars)
                {
                    if (addcar.PhotoVehicle == null) addcar.PhotoVehicle = "car_icon.png";
                    Vehicles.Add(addcar);
                }
            }
            else await _dialogService.ShowToastAsync(result.ErrorMessage);
        } //загрузка списка авто
        [RelayCommand]
        public async void ShowVehicleOptions(Vehicle selectedVehicle)
        {
            if (selectedVehicle == null) return;
            var respon = await _dialogService.ShowDisplayAction();
            if (respon == "Отмена") return;
            if (respon == "Редактировать") await EditVehicle(selectedVehicle);
            else if (respon == "Удалить") await DeleteVehicle(selectedVehicle);
        }

        private async Task DeleteVehicle(Vehicle vehicleSelected)
        {
            var result = await _dataService.DeleteVehicleAsync(vehicleSelected, _cts.Token);
            if (result.Success)
            {
                await _dialogService.ShowToastAsync("ТС удалено");
                await LoadVehicles();
            }
            else await _dialogService.ShowToastAsync(result.ErrorMessage);
        } //удаление авто

        private async Task EditVehicle(Vehicle vehicleSelected)
        {
            var property = new Dictionary<string, object>()
            {
                ["VehicleId"] = vehicleSelected.Id
            };

            await _navigationService.GoNavigation("CreateVehicleView", property);
        } //обновление информации авто

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        } //отмена токена

        private void CheckWarningRepair(IEnumerable<Vehicle> cars)
        {
            foreach (var list in cars)
            {
                var warningCount = IntervalCalculatroService.CalculatingWarning(list);
                if (warningCount > 0)
                {
                    list.WarningRepair = $"Ко-во узлов требующих осомтра:{warningCount}";
                    list.NeedsService = true;
                    IsWarningRepair = true;
                }
                else
                { 
                    list.NeedsService = false;
                    IsWarningRepair = false;
                }
            }
        }  //проверка кол предупреждений о ремонте авто
    }
}
