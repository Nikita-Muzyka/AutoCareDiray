using AutoCareDiray.Service;
using AutoCareDiray.Service.Data;
using AutoCareDiray.Shared.Models.VehicleModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoCareDiray.ViewModels.VehicleViewModel
{
    [QueryProperty(nameof(VehicleId), "VehicleId")]
    public partial class CardVehicleViewModel : BaseViewModel
    {
        private int _vehicleId;
        public int VehicleId
        {
            get => _vehicleId;
            set
            {
                _vehicleId = value;
                OnPropertyChanged();
                LoadVehicle();
            }
        }
        CancellationTokenSource _cts;

        [ObservableProperty]
        private Vehicle vehicleRespon;

        public CardVehicleViewModel(IApiService apiService, IDialogService dialogService,IDataService dataService) 
            : base(apiService, dialogService,dataService)
        {
          _cts = new CancellationTokenSource();
        }


        private async Task LoadVehicle()
        {
            VehicleRespon = await _dataService.GetVehicleAsync(VehicleId, _cts.Token);
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
