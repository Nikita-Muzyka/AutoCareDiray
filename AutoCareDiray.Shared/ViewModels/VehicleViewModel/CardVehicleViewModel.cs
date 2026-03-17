using AutoCareDiray.Shared.Service.Data;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Interface;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.Shared.Service.ResultService;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoCareDiray.Shared.ViewModels.VehicleViewModel
{
    public partial class CardVehicleViewModel : BaseViewModel
    {
        private int _vehicleId;
        private bool isInitilize = false;
       
        CancellationTokenSource _cts;
        [ObservableProperty]
        ObservableCollection<RepairType> warningRepairType;
        [ObservableProperty]
        private Vehicle vehicleRespon;

        public CardVehicleViewModel(IDialogService dialog, IDataService data, INavigationService navigate)
            : base(dialog, data, navigate)
        {
          _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public async Task Initilize(int vehicleId)
        {
            if (isInitilize) return;
            _vehicleId = vehicleId;
            isInitilize = true;
            await LoadVehicle();
        }
        private async Task LoadVehicle()
        {
            var result = await _dataService.GetVehicleAsync(_vehicleId, _cts.Token);
            if(result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                VehicleRespon = resultVehicle.Data ?? new Vehicle();

                var warning = VehicleRespon.RepairTypes.Where(c => VehicleRespon.Mileage - c.LastServiceMileage > c.IntervalMileage).ToList();
                WarningRepairType = new ObservableCollection<RepairType>(warning);
            }
        }
    }
}
