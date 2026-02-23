using AutoCareDiray.Service;
using AutoCareDiray.Service.Data;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Service.Navigation;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.View.RepairView;


namespace AutoCareDiray.ViewModels.RepairViewModel
{
    public partial class ListRepairViewModel : BaseViewModel
    {
        public ObservableCollection<Repair> Repairs { get; set; } = new ObservableCollection<Repair>();
        public ObservableCollection<Vehicle> Vehicles { get; set; } = new ObservableCollection<Vehicle>();

        CancellationTokenSource _cts;

        [ObservableProperty]
        public Vehicle selectedVehicle = new Vehicle();

       
        public ListRepairViewModel(IApiService api, IDialogService dialog, IDataService data, INavigationService navigate)
            : base(api, dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public async void LoadData()
        {
            if(Vehicles is not null) Vehicles.Clear();

            foreach (var vehicle in await _dataService.ListVehicleAsync(_cts.Token))
            {
                if(vehicle is not null)
                {
                    Vehicles.Add(vehicle);
                }
            }
            SelectedVehicle = Vehicles[0];
        }
        async void LoadRepairs()
        {
            if(Repairs is not null) Repairs.Clear();
            foreach (var repairs in await _dataService.ListRepairForVehicleAsync(SelectedVehicle.Id,_cts.Token))
            {
                if (repairs is not null)
                {
                    Repairs.Add(repairs);
                }
            }
        }

        [RelayCommand]
        public async Task GoRepairCard()
        {
            //await _navigationService.GoNavigation();
        }
        [RelayCommand]
        public async Task GoCreateRepair()
        {
            var property = new Dictionary<string, object>()
            {
                ["VehicleId"] = SelectedVehicle.Id,
            };
            await _navigationService.GoNavigation(nameof(CreateRepairView),property);
        }
        partial void OnSelectedVehicleChanged(Vehicle value)
        {
            if(value is not null) LoadRepairs();
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
