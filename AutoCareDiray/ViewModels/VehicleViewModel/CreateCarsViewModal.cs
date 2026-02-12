using AutoCareDiray.Models.Validation;
using AutoCareDiray.Service;
using AutoCareDiray.Models.VehicleModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AutoCareDiray.ViewModels.VehicleViewModel
{
    public partial class CreateVehicleViewModel : BaseViewModel
    {
        private VehicleValidation _vehicleValidation;
        private CancellationTokenSource _cts;


        [ObservableProperty]
        private string nameVehicle;
        [ObservableProperty]
        private string yearCreate;
        [ObservableProperty]
        private string mileage;
        [ObservableProperty]
        private string yearPuchase;
        [ObservableProperty]
        private string selectedTypeVehicle;

        [ObservableProperty]
        private string statusMessage;

        public ObservableCollection<string> TypeVehicle { get; } = new() { "Автомобиль", "Мотоцикл", "Грузовое ТС", "Другое" };



        public CreateVehicleViewModel(IApiService apiService,IDialogService dialogService,VehicleValidation vehicleValidation) : base(apiService, dialogService)
        {
            _vehicleValidation = vehicleValidation;
            _vehicleValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
            _cts = new CancellationTokenSource();
        }

        // Получение ошибок
        public bool HasErrors => _vehicleValidation.HasErrors;
        public string MileageError => _vehicleValidation.GetErrors(nameof(MileageError)) as string;

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
            if (!HasErrors)
            {
                var vehicle = CreateClassVehicle();
            }
        }

        //методы Community Tool
      
        partial void OnMileageChanged(string value)
        {
            _vehicleValidation.ValidationMileage(value);
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

        /// <summary>
        /// Создание авто
        /// </summary>
        /// <returns></returns>
        Vehicle CreateClassVehicle()
        {
           
            int MileageInt = ConverFromInt(Mileage);

            var car = new Vehicle
            {
              NameVehicle = NameVehicle,
            };

            return car;
        }
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
}
