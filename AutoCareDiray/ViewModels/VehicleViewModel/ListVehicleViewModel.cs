using AutoCareDiray.Service;
using AutoCareDiray.View.VehicleView;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.Models.VehicleModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

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
        public ListVehicleViewModel(IApiService apiService, IDialogService dialogService) : base(apiService,dialogService)
        {
            Vehicles = new ObservableCollection<Vehicle>();
        }

        [RelayCommand]
        public async void GoCreateVehicle()
        {
            await Shell.Current.GoToAsync(nameof(CreateVehicleView));
        }
        [RelayCommand]
        public async Task GoCarCard(Vehicle VehicleSelected)
        {
            var property = new Dictionary<string, object>()
            {
                ["SelVehicle"] = VehicleSelected
            };
            await Shell.Current.GoToAsync(nameof(CardVehicleView), property);
        }

        //[RelayCommand]
        /// <summary>
        /// Загрузка авто с сервера
        /// </summary>
        //public async void LoadCars()
        //{

        //    try
        //    {
        //        _cts = new CancellationTokenSource();
        //        var response = await _apiService.GetCarByUserIdApiAsync(_cts.Token);
        //        var carsResponse = response as CarListResponse;

        //        if(carsResponse != null)
        //        {
        //            foreach (var addcar in carsResponse.cars)
        //            {
        //                Vehicles.Add(addcar);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Errors = ex.Message;
        //    }
        //}

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
        [RelayCommand]
        public void CreateToken()
        {
            _cts = new CancellationTokenSource();
        }
    }
}
