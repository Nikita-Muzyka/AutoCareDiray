using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.Data;
using AutoCareDiray.Shared.Service.ResultService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AutoCareDiray.Shared.ViewModels.VehicleViewModel
{
    public partial class CardVehicleViewModel : BaseViewModel
    {
        #region Основые классы и списки

        private int _vehicleId;
        private bool isInitilize = false;

        CancellationTokenSource _cts;

        [ObservableProperty]
        ObservableCollection<RepairType> warningRepairType;

        [ObservableProperty]
        private Vehicle vehicleCard;

        #endregion

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
            await LoadVehicle();
            isInitilize = true;
        } // инициализация карточки
        private async Task LoadVehicle()
        {
            var result = await _dataService.GetVehicleAsync(_vehicleId, _cts.Token);
            if(result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                VehicleCard = resultVehicle.Data ?? new Vehicle();
                var sortRepairType = VehicleCard.RepairTypes.Where(c => c.IntervalMileage > 0).ToList();
                var warning = sortRepairType.Where(c => VehicleCard.Mileage - c.LastServiceMileage > c.IntervalMileage).ToList();
                WarningRepairType = new ObservableCollection<RepairType>(warning);
            }
        } // загрузка информации по авто
    }
}
