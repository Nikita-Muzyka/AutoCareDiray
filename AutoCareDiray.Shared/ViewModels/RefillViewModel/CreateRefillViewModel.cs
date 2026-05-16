using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RefillModel;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.Validation;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.ResultService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.ViewModels.RefillViewModel
{
    public partial class CreateRefillViewModel : BaseViewModel
    {
        #region основные классы

        private readonly IPhotoPicker _photoPicker;
        private readonly RefillValidation _refillValidation;

        private CancellationTokenSource _cts;
        private bool _isInitilize = false;
        private bool _isUpdate = false;
        private int _vehicleId;
        private int _refillId;
        private Vehicle _vehicle;
        private Refill _refill;


        #endregion

        #region для работы UI

        public ObservableCollection<string> AttachedPhotos { get; set; }


        [ObservableProperty]
        private List<string> fuelTypes = new List<string>() { "92", "95", "98", "100", "ДТ" };

        [ObservableProperty]
        private DateTime dateRefillSelected = DateTime.UtcNow;

        [ObservableProperty]
        private int mileageFilled;
        [ObservableProperty]
        private double volumeLitersFilled;
        [ObservableProperty]
        private decimal costFilled;

        [ObservableProperty]
        private bool isFullTank;
        [ObservableProperty]
        private bool isMileageError = false;
        [ObservableProperty]
        private bool isVolumeLitersError = false;
        [ObservableProperty]
        private bool isCostError = false;
        [ObservableProperty]
        private bool isFuelTypesError = false;

        [ObservableProperty]
        private string buttonName = "Добавить";
        [ObservableProperty]
        private string selectedFuelType;
        [ObservableProperty]
        private string gasStationName;
        [ObservableProperty]
        private string descriptionFilled;
        [ObservableProperty]
        private string statusMessage;


        #endregion
        public CreateRefillViewModel(IDialogService dialog, IDataService data, INavigationService navigate,IPhotoPicker photo,RefillValidation valid) :base(dialog,data,navigate)
        {
            _cts = new CancellationTokenSource();
            _refillValidation = valid;
            _refillValidation.ErrorsChanged +=  (s,e) => OnErrorsChangedUI(e);
        }

        public bool HasErrors => _refillValidation.HasErrors;
        public string MileageError => _refillValidation.GetErrors(nameof(MileageError)) as String;
        public string CostError => _refillValidation.GetErrors(nameof(CostError)) as String;
        public string VolumeLitersError => _refillValidation.GetErrors(nameof(VolumeLitersError)) as String;
        public string FuelTypesError => _refillValidation.GetErrors(nameof(FuelTypesError)) as String;


        [RelayCommand]
        public async Task Initialize(IDictionary<string, object> query)
        {
            if (_isInitilize) return;
            if (query.TryGetValue("VehicleId", out var obj))
            {
                if (obj is int value)
                {
                    _vehicleId = value;
                    _isInitilize = true;
                    await LoadingCreate();
                }
            }
            else if (query.TryGetValue("RefillId", out var objTwo))
            {
                if (objTwo is int value)
                {
                    _refillId = value;
                    _isInitilize = true;
                    await LoadingUpdate();
                }
            }
        } //Инициализация и определения редактирования или обновления данных

        public async Task LoadingUpdate()
        {
            var result = await _dataService.GetRefillAsync(_refillId, _cts.Token); // изменить
            if (result.Success)
            {
                var resultRefill = result as Result<Refill>;
                _isUpdate = true;
                ButtonName = " Редактировать";

                DateRefillSelected = resultRefill.Data.DateRefill;
                MileageFilled = resultRefill.Data.Mileage;
                VolumeLitersFilled = resultRefill.Data.VolumeLiters;
                CostFilled = resultRefill.Data.Cost;
                IsFullTank = resultRefill.Data.IsFullTank;
                SelectedFuelType = resultRefill.Data.FuelType;
                GasStationName = resultRefill.Data.GasStationName;
                DescriptionFilled = resultRefill.Data.Description;

                _vehicleId = resultRefill.Data.VehicleId;


                if (resultRefill.Data.Photos != null)
                {
                    AttachedPhotos ??= new ObservableCollection<string>();

                    foreach (var photo in resultRefill.Data.Photos)
                    {
                        if (File.Exists(photo)) AttachedPhotos.Add(photo);
                    }
                }
            }
            else StatusMessage = result.ErrorMessage;
        } //Загрузка ресурсов под редактирования 

        public async Task LoadingCreate()
        {
            
            var result = await _dataService.GetVehicleAsync(_vehicleId, _cts.Token); // изменить
            if (result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                _vehicle = resultVehicle.Data;
                _vehicleId = resultVehicle.Data.Id;
                MileageFilled = _vehicle.Mileage;
            }
            else StatusMessage = result.ErrorMessage;
        } //Загрузка под создания ремонта

        [RelayCommand]
        public async Task ProcessingRefill()
        {
            _refillValidation.ValidationAll(MileageFilled, CostFilled,SelectedFuelType,VolumeLitersFilled); // изменить
            if (HasErrors) return;
            _refill = CreateRefill();

            var result = await _photoPicker.SavePhotosAsync(AttachedPhotos, _cts.Token);
            if (result.Success)
            {
                var resultPhotos = result as Result<List<string>>;
                _refill.Photos = resultPhotos.Data;
            }

            if (_isUpdate)
            {
                _refill.Id = _refillId;
                _refill.VehicleId = _vehicleId;
                var resultUdateRepair = await _dataService.UpdateRefillAsync(_refill, _cts.Token); // изменить
                if (resultUdateRepair.Success == false)
                {
                    StatusMessage = resultUdateRepair.ErrorMessage;
                    return;
                }
                _isUpdate = false;
                ButtonName = "Добавить";
            }
            else
            {
                _refill.VehicleId = _vehicleId;
                var resultCreateRepair = await _dataService.CreateRefillAsync(_refill, _cts.Token);
                if (resultCreateRepair.Success == false)
                {
                    StatusMessage = resultCreateRepair.ErrorMessage;
                    return;
                }
            }

            await UpdateDate();
            string message = _isUpdate ? "Заправка отредактирована" : "Заправка добавлена"; 
            await _dialogService.ShowToastAsync(message);
            await _navigationService.GoToBack();
        }  //Создание или редактирование ремонта

        private Refill CreateRefill()
        {
            return new Refill
            {
                DateRefill = DateRefillSelected,
                Mileage = MileageFilled,
                VolumeLiters = VolumeLitersFilled,
                Cost = CostFilled,
                IsFullTank = IsFullTank,
                FuelType = SelectedFuelType,
                GasStationName = GasStationName,
                Description = DescriptionFilled,
            };
        } //Создание объекта ремонта для отправки на сервер

        private async Task UpdateDate()
        {
            var result = await _dataService.GetVehicleMileageAsync(_vehicleId, _cts.Token);
            if (result.Success)
            {
                var resultVeh = result as Result<Vehicle>;
                _vehicle = resultVeh.Data;
            }
            if (MileageFilled > _vehicle.Mileage)
            {
                var resultMileage = await _dataService.UpdateVehicleMileageAsync(_vehicleId, MileageFilled, _cts.Token);
                if (resultMileage.Success == false)
                {
                    StatusMessage = resultMileage.ErrorMessage;
                }

            }

            if (IsFullTank)
            {
               //изменить
            }
        } //Обновление данных автомобиля после создания или редактирования ремонта


        partial void OnIsFullTankChanged(bool value)
        {
            if (value) VolumeLitersFilled = _vehicle.FuelTank;
        }
        partial void OnVolumeLitersFilledChanged(double value)
        {
            IsVolumeLitersError = _refillValidation.ValidationVolumeLiters(value);
        }
        partial void OnSelectedFuelTypeChanged(string value)
        {
            IsFuelTypesError = _refillValidation.ValidationFuelTypes(value);
        }
        partial void OnMileageFilledChanged(int value)
        {
            IsMileageError = _refillValidation.ValidationMileage(value);
        }
        partial void OnCostFilledChanged(decimal value)
        {
            IsCostError = _refillValidation.ValidationCost(value);
        }

        void OnErrorsChangedUI(DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(e.PropertyName);
        }

        [RelayCommand]
        public async Task AttachPhoto()
        {
            var result = await _photoPicker.PickPhotosAsync();

            if (result.Success == false)
            {
                await _dialogService.ShowToastAsync(result.ErrorMessage);
                return;
            }

            var resultPhoto = result as Result<List<string>>;
            AttachedPhotos ??= new();

            foreach (var listPhoto in resultPhoto.Data)
            {
                AttachedPhotos.Add(listPhoto);
            }

        } //выбор фото

        [RelayCommand]
        public async Task DeleteAttachPhoto(string photo)
        {
            AttachedPhotos?.Remove(photo);
        } //удаление фото

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public void EventOff()
        {
            _refillValidation.ErrorsChanged -= (s, e) => OnErrorsChangedUI(e);
        }
    }
}
