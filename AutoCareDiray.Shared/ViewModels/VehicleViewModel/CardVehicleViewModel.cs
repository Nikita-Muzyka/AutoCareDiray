using AutoCareDiray.Shared.Service.Data;
using AutoCareDiray.Shared.Models.VehicleModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AutoCareDiray.Shared.Interface;
using CommunityToolkit.Mvvm.Input;

namespace AutoCareDiray.Shared.ViewModels.VehicleViewModel
{
    public partial class CardVehicleViewModel : BaseViewModel
    {
        private int _vehicleId;
        private bool isInitilize = false;
       
        CancellationTokenSource _cts;

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
            VehicleRespon = await _dataService.GetVehicleAsync(_vehicleId, _cts.Token);
        }
        //[RelayCommand]
        //public async void CreateMaintenanse()
        //{
        //    var Car = new Dictionary<string, object>()
        //    {
        //        ["Car"] = CarSelected
        //    };
        //    await Shell.Current.GoToAsync(nameof(CreateMaintenanse), Car);
        //}
        //[RelayCommand]
        //public async void ListMaintenanse()
        //{

        //    await Shell.Current.GoToAsync(nameof(ListMaintenanseView));
        //}
        //[RelayCommand]
        //public async void GoBack()
        //{
        //    await Shell.Current.GoToAsync("..");
        //}
    }
}
