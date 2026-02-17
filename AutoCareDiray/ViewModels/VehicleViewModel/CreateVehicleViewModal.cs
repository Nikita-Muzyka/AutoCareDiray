using AutoCareDiray.Models.Validation;
using AutoCareDiray.Service;
using AutoCareDiray.Shared.Models.VehicleModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AutoCareDiray.Service.Data;


namespace AutoCareDiray.ViewModels.VehicleViewModel
{
    public partial class CreateVehicleViewModel : BaseViewModel
    {
        private VehicleValidation _vehicleValidation;
        private CancellationTokenSource _cts;


        [ObservableProperty]
        private string nameVehicle;
        [ObservableProperty]
        private DateTime yearCreateSelected = DateTime.Today;
        [ObservableProperty]
        private string mileage;
        [ObservableProperty]
        private DateTime yearPurchaseSelected = DateTime.Today;
        [ObservableProperty]
        private string selectedTypeVehicle;
        [ObservableProperty]
        private DateTime dateNow = DateTime.Today;

        [ObservableProperty]
        private string statusMessage;

        public ObservableCollection<string> TypeVehicle { get; } = new() { "Автомобиль", "Мотоцикл", "Грузовое ТС", "Другое" };



        public CreateVehicleViewModel(IApiService apiService,IDialogService dialogService,IDataService dataService,VehicleValidation vehicleValidation) 
            : base(apiService, dialogService,dataService)
        {
            _vehicleValidation = vehicleValidation;
            _vehicleValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
            _cts = new CancellationTokenSource();
        }

        // Получение ошибок
        public bool HasErrors => _vehicleValidation.HasErrors;
        public string MileageError => _vehicleValidation.GetErrors(nameof(MileageError)) as string;
        public string YearPurchaseError => _vehicleValidation.GetErrors(nameof(YearPurchaseError)) as string;

        /// <summary>
        /// Конвертация данных для создания авто
        /// </summary>
        Func<string, int> ConverFromInt = (property) =>
        {
            if (int.TryParse(property, out int result))
            {
                return result;
            }
            else return 0;
        };


        [RelayCommand]
        public async void CreateVehicle()
        {
            _vehicleValidation.ValidationMileage(Mileage);
            _vehicleValidation.ValidationDate(YearPurchaseSelected, YearCreateSelected);
            if (!HasErrors)
            {
                int MileageInt = ConverFromInt(Mileage);

                var vehicle = new Vehicle
                {
                    NameVehicle = NameVehicle,
                    Mileage = MileageInt,
                    YearCreate = DateOnly.FromDateTime(YearCreateSelected),
                    YearPurchase = DateOnly.FromDateTime(YearPurchaseSelected),
                };
                if(_dataService is null)
                {

                }
                    await _dataService.CreateVehicleAsync(vehicle,_cts.Token);
                await Shell.Current.GoToAsync("..");
            }
        }

        //методы Community Tool
      
        partial void OnMileageChanged(string value)
        {
            _vehicleValidation.ValidationMileage(value);
        }
        partial void OnYearCreateSelectedChanged(DateTime value)
        {
            _vehicleValidation.ValidationDate(YearPurchaseSelected, YearCreateSelected);
        }
        partial void OnYearPurchaseSelectedChanged(DateTime value)
        {
            _vehicleValidation.ValidationDate(YearPurchaseSelected, YearCreateSelected);
        }

        /// <summary>
        /// Метод которые вызывает event 
        /// </summary>
        /// <param name="e"></param>
        void OnErrorsChangedUI(DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(e.PropertyName);
        }

        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
}
