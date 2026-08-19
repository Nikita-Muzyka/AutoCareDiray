using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RefillModel;
using AutoCareDiray.Shared.Models.Validation;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.ResultService;
using AutoCareDiray.Shared.Models.SettignsModel; // Для CurrencyList
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AutoCareDiray.Shared.ViewModels.RefillViewModel
{
    public partial class CreateRefillViewModel : BaseViewModel
    {
        #region Поля и приватные данные

        private readonly IPhotoPicker _photoPicker;
        private readonly RefillValidation _refillValidation;
        private readonly IPreferencesService _preferencesService; // Добавили Preferences

        private CancellationTokenSource _cts;
        private bool _isInitialized = false;
        private bool _isUpdate = false;

        private int _vehicleId = -1;
        private int _refillId = -1;

        private Vehicle _vehicle;
        private Refill _refill;

        #endregion

        #region Коллекции для UI

        public ObservableCollection<string> AttachedPhotos { get; set; } = new();
        public ObservableCollection<string> FuelTypes { get; set; } = new(new[] { "92", "95", "98", "100", "ДТ", "Газ", "Электричество" });
        public ObservableCollection<string> CurrencyPicker { get; set; } = new(); // Список валют

        #endregion

        #region Observable-свойства (данные формы)

        [ObservableProperty] private DateTime dateRefillSelected = DateTime.UtcNow;
        [ObservableProperty] private double mileageFilled;
        [ObservableProperty] private double volumeLitersFilled;
        [ObservableProperty] private decimal costFilled;
        [ObservableProperty] private bool isFullTank;

        [ObservableProperty] private string selectedFuelType;
        [ObservableProperty] private string gasStationName;
        [ObservableProperty] private string descriptionFilled;
        [ObservableProperty] private string statusMessage;
        [ObservableProperty] private string buttonName = "Добавить заправку";

        [ObservableProperty] private string selectedCurrencySign; // Знак валюты
        [ObservableProperty] private string mileageUnit = "км"; // Единицы измерения

        #endregion

        #region Observable-свойства (флаги ошибок)

        [ObservableProperty] private bool isMileageError = false;
        [ObservableProperty] private bool isVolumeLitersError = false;
        [ObservableProperty] private bool isCostError = false;
        [ObservableProperty] private bool isFuelTypesError = false;

        #endregion

        public CreateRefillViewModel(
            IDialogService dialog,
            IDataService data,
            INavigationService navigate,
            IPhotoPicker photo,
            RefillValidation valid,
            IPreferencesService preferencesService) // Внедряем сервис настроек
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
            _refillValidation = valid;
            _refillValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
            _photoPicker = photo;
            _preferencesService = preferencesService;
        }

        #region Свойства ошибок валидации

        public bool HasErrors => _refillValidation.HasErrors;
        public string MileageError => _refillValidation.GetErrors(nameof(MileageError)) as string;
        public string CostError => _refillValidation.GetErrors(nameof(CostError)) as string;
        public string VolumeLitersError => _refillValidation.GetErrors(nameof(VolumeLitersError)) as string;
        public string FuelTypesError => _refillValidation.GetErrors(nameof(FuelTypesError)) as string;

        #endregion

        #region Инициализация и загрузка данных

        [RelayCommand]
        public async Task Initialize(IDictionary<string, object> query)
        {
            if (_isInitialized) return;

            // Загружаем список валют
            CurrencyPicker = new ObservableCollection<string>(CurrencyList.GetListMoneySign());
            SelectedCurrencySign = _preferencesService.GetDefaultMoneySign() ?? "₽";

            if (query.TryGetValue("VehicleId", out var vehicleIdObj) && vehicleIdObj is int vehicleId)
            {
                _vehicleId = vehicleId;
                _isInitialized = true;
                await LoadingCreate();
            }
            else if (query.TryGetValue("RefillId", out var refillIdObj) && refillIdObj is int refillId)
            {
                _refillId = refillId;
                _isInitialized = true;
                await LoadingUpdate();
            }
        }

        public async Task LoadingCreate()
        {
            var result = await _dataService.GetVehicleAsync(_vehicleId, _cts.Token);

            if (result is not Result<Vehicle> vehicleResult)
            {
                StatusMessage = result.ErrorMessage;
                return;
            }

            _vehicle = vehicleResult.Data;
            _vehicleId = _vehicle.Id;
            MileageFilled = _vehicle.Mileage;
            MileageUnit = _vehicle.UnitDistance ?? "км"; // Берем единицы измерения у авто
        }

        public async Task LoadingUpdate()
        {
            var result = await _dataService.GetRefillAsync(_refillId, _cts.Token);

            if (result is not Result<Refill> refillResult)
            {
                StatusMessage = result.ErrorMessage;
                return;
            }

            _isUpdate = true;
            ButtonName = "Сохранить изменения";
            var data = refillResult.Data;

            DateRefillSelected = data.DateRefill;
            MileageFilled = data.Mileage;
            VolumeLitersFilled = data.VolumeLiters;
            CostFilled = data.Cost;
            IsFullTank = data.IsFullTank;
            SelectedFuelType = data.FuelType;
            GasStationName = data.GasStationName;
            DescriptionFilled = data.Description;
            _vehicleId = data.VehicleId;

            if (data.Photos != null)
            {
                AttachedPhotos.Clear();
                foreach (var photo in data.Photos)
                {
                    if (File.Exists(photo))
                        AttachedPhotos.Add(photo);
                }
            }
        }

        #endregion

        #region Главная команда: Сохранение

        [RelayCommand]
        public async Task ProcessingRefill()
        {
            ValidationAll();
            if (HasErrors) return;

            _refill = CreateRefillEntity();

            if (AttachedPhotos?.Any() == true)
            {
                var photoResult = await _photoPicker.SavePhotosAsync(AttachedPhotos, _cts.Token);
                if (photoResult is Result<List<string>> typedPhotoResult)
                {
                    _refill.Photos = typedPhotoResult.Data;
                }
            }

            Result resultSave;
            if (_isUpdate)
            {
                _refill.Id = _refillId;
                resultSave = await _dataService.UpdateRefillAsync(_refill, _cts.Token);
            }
            else
            {
                resultSave = await _dataService.CreateRefillAsync(_refill, _cts.Token);
            }

            if (!resultSave.Success)
            {
                StatusMessage = resultSave.ErrorMessage;
                return;
            }

            await TryUpdateVehicleDataAsync();

            string message = _isUpdate ? "Заправка отредактирована" : "Заправка добавлена";
            await _dialogService.ShowToastAsync(message);
            await _navigationService.GoToBack();
        }

        private Refill CreateRefillEntity()
        {
            return new Refill
            {
                VehicleId = _vehicleId,
                DateRefill = DateRefillSelected,
                Mileage = MileageFilled,
                VolumeLiters = VolumeLitersFilled,
                Cost = CostFilled,
                IsFullTank = IsFullTank,
                FuelType = SelectedFuelType,
                GasStationName = GasStationName,
                Description = DescriptionFilled,
            };
        }

        private async Task TryUpdateVehicleDataAsync()
        {
            var result = await _dataService.GetVehicleMileageAsync(_vehicleId, _cts.Token);
            if (result is Result<Vehicle> vehResult)
            {
                _vehicle = vehResult.Data;
            }

            // Обновляем пробег, если ввели больше
            if (_vehicle != null && MileageFilled > _vehicle.Mileage)
            {
                var resultMileage = await _dataService.UpdateVehicleMileageAsync(_vehicleId, MileageFilled, _cts.Token);
                if (!resultMileage.Success)
                    StatusMessage = resultMileage.ErrorMessage;
            }

            if (IsFullTank)
            {
                // Логика расчета полного бака
            }
        }

        #endregion

        #region Частичные методы (Ошибки и Триггеры)

        private void ValidationAll()
        {
            IsVolumeLitersError = _refillValidation.ValidationVolumeLiters(VolumeLitersFilled);
            IsFuelTypesError = _refillValidation.ValidationFuelTypes(SelectedFuelType);
            IsMileageError = _refillValidation.ValidationMileage(MileageFilled);
            IsCostError = _refillValidation.ValidationCost(CostFilled);
        }

        partial void OnIsFullTankChanged(bool value)
        {
            if (value)
            {
                // Если ВКЛЮЧИЛИ полный бак -> ставим объем бака авто
                if (_vehicle != null)
                {
                    VolumeLitersFilled = _vehicle.FuelTank;
                }
            }
            else
            {
                // Если ВЫКЛЮЧИЛИ -> сбрасываем значение в 0, чтобы можно было ввести вручную
                VolumeLitersFilled = 0;
            }
        }

        partial void OnVolumeLitersFilledChanged(double value) => IsVolumeLitersError = _refillValidation.ValidationVolumeLiters(value);
        partial void OnSelectedFuelTypeChanged(string value) => IsFuelTypesError = _refillValidation.ValidationFuelTypes(value);
        partial void OnMileageFilledChanged(double value) => IsMileageError = _refillValidation.ValidationMileage(value);
        partial void OnCostFilledChanged(decimal value) => IsCostError = _refillValidation.ValidationCost(value);

        private void OnErrorsChangedUI(DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(e.PropertyName);
        }

        #endregion

        #region Работа с Фотографиями

        [RelayCommand]
        public async Task AttachPhoto()
        {
            var result = await _photoPicker.PickPhotosAsync();

            if (!result.Success)
            {
                await _dialogService.ShowToastAsync(result.ErrorMessage);
                return;
            }

            if (result is Result<List<string>> typedResult)
            {
                foreach (var photo in typedResult.Data)
                    AttachedPhotos.Add(photo);
            }
        }

        [RelayCommand]
        public void DeleteAttachPhoto(string photo)
        {
            AttachedPhotos?.Remove(photo);
        }

        #endregion

        #region Токен отмены и Отписка

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

        #endregion
    }
}