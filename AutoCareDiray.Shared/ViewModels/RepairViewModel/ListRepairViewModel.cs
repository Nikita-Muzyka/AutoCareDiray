using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Service.ResultService;   


namespace AutoCareDiray.Shared.ViewModels.RepairViewModel
{
    public partial class ListRepairViewModel : BaseViewModel
    {
        public ObservableCollection<Repair> Repairs { get; set; } = new ObservableCollection<Repair>();
        public ObservableCollection<Vehicle> Vehicles { get; set; } = new ObservableCollection<Vehicle>();

        CancellationTokenSource _cts;

        [ObservableProperty]
        public Vehicle selectedVehicle = new Vehicle();


        public ListRepairViewModel(IDialogService dialog, IDataService data, INavigationService navigate)
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public async Task StartLoading()
        {
            if (Vehicles.Count > 0) Vehicles.Clear();
            var result = await _dataService.ListVehicleForListRepairAsync(_cts.Token);
            if (result.Success)
            {
                var resultVehicles = result as Result<List<Vehicle>>;
                foreach (var addcar in resultVehicles.Data)
                {
                    Vehicles.Add(addcar);
                }
                SelectedVehicle = Vehicles.FirstOrDefault() ?? new Vehicle();
                await LoadRepairs();
            }
            else await _dialogService.ShowToastAsync(result.ErrorMessage);
        }

        private async Task LoadRepairs()
        {
            if (Repairs.Count > 0) Repairs.Clear();
            var result = await _dataService.ListRepairForVehicleAsync(SelectedVehicle.Id, _cts.Token);
            if (result.Success)
            {
                var resultRepairs = result as Result<List<Repair>>;
                foreach (var repairs in resultRepairs.Data)
                {
                    Repairs.Add(repairs);
                }
            }
            else await _dialogService.ShowToastAsync(result.ErrorMessage);
        }

        [RelayCommand]
        public async Task GoRepairCard(Repair repair)
        {
            var parametr = new Dictionary<string, object>()
            {
                ["RepairId"] = repair.Id,
            };
            await _navigationService.GoNavigation("CardRepairView", parametr);
        }
        [RelayCommand]
        public async Task GoCreateRepair()
        {
            var property = new Dictionary<string, object>()
            {
                ["VehicleId"] = SelectedVehicle.Id,
            };
            await _navigationService.GoNavigation("CreateRepairView", property);
        }
        //partial void OnSelectedVehicleChanged(Vehicle value)
        //{
        //    if (value is not null) LoadRepairs(value.Repairs);
        //}

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public async Task DeleteRepair(Repair repair)
        {
            await _dataService.DeleteRepairAsync(repair, _cts.Token);
            await LoadRepairs();
        }

        [RelayCommand]
        public async Task UpdateRepair(Repair repair)
        {
            var parametr = new Dictionary<string, object>()
            {
                ["RepairId"] = repair.Id
            };

            await _navigationService.GoNavigation("CreateRepairView", parametr);
        }
    }
}
