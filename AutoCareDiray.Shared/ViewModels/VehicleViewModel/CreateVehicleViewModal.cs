using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.Validation;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.Data;
using AutoCareDiray.Shared.Service.ResultService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.IO;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace AutoCareDiray.Shared.ViewModels.VehicleViewModel
{
    public partial class CreateVehicleViewModel : BaseViewModel
    {
        #region Основные поля для работы

        private readonly VehicleValidation _vehicleValidation;
        private readonly IPhotoPicker _photoPicker;
        private readonly IPreferencesService _preferencesService;
        private CancellationTokenSource _cts;
        private Vehicle _vehicle;
        //Инициализирована ли страница
        private bool _isInitilized = false;
        //переключатель с создания авто на обновление данных авто
        private bool _isUpdateVehicle = false;
        #endregion


        #region Свойства для работы UI

        [ObservableProperty]
        private bool isYearPurchaseError = false;
        [ObservableProperty]
        private bool isMileageError = false;
        [ObservableProperty]
        private bool isTypeVehicleError = false;
        [ObservableProperty]
        private bool isNameVehicleError = false;
        [ObservableProperty]
        private bool isFuelTankError = false;


        [ObservableProperty]
        private string nameVehicle = String.Empty;
        [ObservableProperty]
        private string vinCode = String.Empty;
        [ObservableProperty]
        private string stateNumber = String.Empty;
        [ObservableProperty]
        private string selectedTypeVehicle = String.Empty;
        [ObservableProperty]
        private string transmissionType = String.Empty;
        [ObservableProperty]
        private string pathPhoto = "car_add_icon.png";
        [ObservableProperty]
        private string buttonName = "Создать";
        [ObservableProperty]
        private string statusMessage;

        [ObservableProperty]
        private int mileage;
        [ObservableProperty]
        private double fuelTank;

        public string FuelTankText => "Введите обьем бака в ( " + _preferencesService.GetDefaultShortVolume() + " )";
        public string MileageText => "Введите текущий пробег авто в ( " + _preferencesService.GetDefaultShortDistance() + " )";


        [ObservableProperty]
        private DateTime dateNow = DateTime.Today;
        [ObservableProperty]
        private DateTime yearPurchaseSelected = new DateTime(1970, 1, 1);

        private string _newPhoto;


       
        #endregion


        public CreateVehicleViewModel(IDialogService dialogService,IDataService dataService,INavigationService navigation,IPhotoPicker photoPicker,
            VehicleValidation vehicleValidation,IPreferencesService preferences) : base(dialogService,dataService,navigation)
        {
            _vehicleValidation = vehicleValidation;
            _photoPicker = photoPicker;
            _preferencesService = preferences;
            _vehicleValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
            _cts = new CancellationTokenSource();
        }


        #region Получение текса ошибок

        public bool HasErrors => _vehicleValidation.HasErrors;
        public string MileageError => _vehicleValidation.GetErrors(nameof(MileageError)) as string ?? String.Empty;
        public string YearPurchaseError => _vehicleValidation.GetErrors(nameof(YearPurchaseError)) as string ?? String.Empty;
        public string TypeVehicleError => _vehicleValidation.GetErrors(nameof(TypeVehicleError)) as string ?? String.Empty;
        public string NameVehicleError => _vehicleValidation.GetErrors(nameof(NameVehicleError)) as string ?? String.Empty;
        public string FuelTankError => _vehicleValidation.GetErrors(nameof(FuelTankError)) as string ?? String.Empty;

        #endregion


        /// <summary>
        /// Инициализация при обновлении данных авто
        /// </summary>
        /// <param name="VehicleId"></param>
        /// <returns></returns>
        [RelayCommand]
        public async Task InitilizeUpdateVeicle(int VehicleId)
        {
            if (_isInitilized) return;
            if (VehicleId < 0) return;

            var result = await _dataService.GetVehicleAsync(VehicleId, _cts.Token);
            if (result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                _vehicle = resultVehicle.Data ?? new Vehicle();

                _isUpdateVehicle = true;
                _isInitilized = true;
                ButtonName = "Изменить";

                if (File.Exists(_vehicle.PhotoVehicle)) PathPhoto = _vehicle.PhotoVehicle;
                NameVehicle = _vehicle.NameVehicle;
                YearPurchaseSelected = _vehicle.YearPurchase;
                Mileage = _vehicle.Mileage;
                SelectedTypeVehicle = _vehicle.VehicleType;
                VinCode = _vehicle.VinCode;
                FuelTank = _vehicle.FuelTank;
                if (string.IsNullOrWhiteSpace(_vehicle.TransmissionType) == false) TransmissionType = _vehicle.TransmissionType;
            }
        }

        /// <summary>
        /// Инициализация при создании авто
        /// </summary>
        [RelayCommand]
        public async Task InitilizeForCreateVeicle()
        {
            if (_isInitilized) return;
        }

        /// <summary>
        /// Создание авто
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task ProcessingAsync()
        {
            ValidationAll();
            if (HasErrors)
            {
                await _dialogService.ShowToastAsync("Ошибка. Проверте все поля");
                return;
            }

            var vehicle = new Vehicle
                (NameVehicle,
                YearPurchaseSelected, VinCode,
                StateNumber, TransmissionType,
                SelectedTypeVehicle, Mileage,
                FuelTank,null);

            if (_newPhoto != null)
            {
                var result = await _photoPicker.SavePhotoAsync(_newPhoto, _cts.Token);
                if (result.Success)
                {
                    var resultPhoto = result as Result<string>;
                    vehicle.PhotoVehicle = resultPhoto.Data;

                    if(_isUpdateVehicle) await _photoPicker.DeletePhoto(_vehicle.PhotoVehicle);
                }
                else await _dialogService.ShowToastAsync(result.ErrorMessage);
            } //save photo

            if (_isUpdateVehicle)
            {
                vehicle.Id = _vehicle.Id;
                await EditVehicle(vehicle);
            } // переход на обновление данных авто
            else
            {
                var result = await _dataService.CreateVehicleAsync(vehicle, _cts.Token);

                if (result.Success)
                {
                    await _dialogService.ShowToastAsync("ТС создано");
                    await _navigationService.GoToBack();
                }
                else StatusMessage = result.ErrorMessage;
            }
        }

        /// <summary>
        /// Обновление данных авто
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public async Task EditVehicle(Vehicle vehicle)
        {
            var result = await _dataService.UpdateVehicleAsync(vehicle, _cts.Token);
            if (result.Success)
            {
                await _dialogService.ShowToastAsync("Данные обновлены");
                await _navigationService.GoToBack();
            }
            else StatusMessage = result.ErrorMessage;
        }

        /// <summary>
        /// Добавление фото для авто
        /// </summary>
        [RelayCommand]
        public async void PickPhoto()
        {
            var result = await _photoPicker.PickPhotoAsync();
            if (result.Success)
            {
                var photoLocation = result as Result<string>;
                PathPhoto = photoLocation.Data;
                _newPhoto = PathPhoto;
            }
        }


        #region Методы CommunityToolKit

        partial void OnMileageChanged(int value)
        {
            IsMileageError = _vehicleValidation.ValidationMileage(value);
        }
        partial void OnYearPurchaseSelectedChanged(DateTime value)
        {
           IsYearPurchaseError = _vehicleValidation.ValidationDate(YearPurchaseSelected);
        }
        partial void OnSelectedTypeVehicleChanged(string value)
        {
           IsTypeVehicleError = _vehicleValidation.ValidationTypeVehicle(value);
        }
        partial void OnNameVehicleChanged(string value)
        {
           IsNameVehicleError = _vehicleValidation.ValidationNameVehicle(value);
        }
        partial void OnFuelTankChanged(double value)
        {
            IsFuelTankError = _vehicleValidation.ValidationFuelTank(value);
        }

        #endregion

        private void ValidationAll()
        {
            IsMileageError = _vehicleValidation.ValidationMileage(Mileage);
            IsYearPurchaseError =  _vehicleValidation.ValidationDate(YearPurchaseSelected);
            IsTypeVehicleError = _vehicleValidation.ValidationTypeVehicle(SelectedTypeVehicle);
            IsNameVehicleError = _vehicleValidation.ValidationNameVehicle(NameVehicle);
            IsFuelTankError = _vehicleValidation.ValidationFuelTank(FuelTank);
        }

        /// <summary>
        /// Метод которые вызывает event 
        /// </summary>
        /// <param name="e"></param>
        private void OnErrorsChangedUI(DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(e.PropertyName);
        }
       
        /// <summary>
        /// Удаление токена
        /// </summary>
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Отписка от эвента
        /// </summary>
        [RelayCommand]
        public void OffEvent()
        {
            _vehicleValidation.ErrorsChanged -= (s,e) => OnErrorsChangedUI(e);
        }

    }
}
