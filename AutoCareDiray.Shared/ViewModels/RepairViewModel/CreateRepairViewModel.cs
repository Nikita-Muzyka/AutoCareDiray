using AutoCareDiray.Shared.Extensions.RepairEx;
using AutoCareDiray.Shared.Extensions.StringEx;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.SettignsModel;
using AutoCareDiray.Shared.Models.Validation;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.ResultService;
using AutoCareDiray.Shared.Service.ValidationService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;


namespace AutoCareDiray.Shared.ViewModels.RepairViewModel
{
    public partial class CreateRepairViewModel : BaseViewModel
    {
        #region Поля и приватные данные

        private CancellationTokenSource _cts;
        private readonly RepairValidation _validationRepair;
        private readonly IPhotoPicker _photoPicker;
        private readonly IPreferencesService _preferencesService;

        private Repair _repair;
        private Vehicle _vehicle;
        private RepairType _newRepairType; // Переименовал: понятнее, что это новый тип

        private int _vehicleId = -1;
        private int _repairId = -1;

        private bool _isInitialized = false;
        private bool _isUpdateRepair = false;

        #endregion

        #region Коллекции для UI

        public ObservableCollection<RepairType> RepairTypes { get; set; } = new();
        public ObservableCollection<string> Categories { get; set; } = new();
        public ObservableCollection<string> AttachedPhotos { get; set; } = new();
        public ObservableCollection<SparePart> ListSpareParts { get; set; } = new();
        public ObservableCollection<string> CurrencyPicker { get; set; } = new();

        #endregion

        #region Observable-свойства (данные формы)

        [ObservableProperty] private RepairType selectedRepairType;
        [ObservableProperty] private int intervalMileageFilled;
        [ObservableProperty] private int intervalMonthsFilled;
        [ObservableProperty] private double mileageFilled;
        [ObservableProperty] private decimal costFilled;
        [ObservableProperty] private decimal createCostPart;
        [ObservableProperty] private int intervalMileageNewType;
        [ObservableProperty] private int intervalMonthNewType;
        [ObservableProperty] private DateTime dateRepairSelected = DateTime.UtcNow;
        [ObservableProperty] private string selectedSparePart;
        [ObservableProperty] private string descriptionFilled;
        [ObservableProperty] private string statusMessage;
        [ObservableProperty] private string buttonName = "Создать";
        [ObservableProperty] private string selectedJob;
        [ObservableProperty] private string serviceName;
        [ObservableProperty] private string commentMechanic;
        [ObservableProperty] private string createNamePart;
        [ObservableProperty] private string createArticleNumberPart;
        [ObservableProperty] private string selectedCategory;
        [ObservableProperty] private string titleNewRepairType;
        [ObservableProperty] private string selectedCurrencySignPart;
        [ObservableProperty] private string selectedCurrencySignAllCost;

        #endregion

        #region Observable-свойства (флаги UI)

        [ObservableProperty] private bool isMileageError = false;
        [ObservableProperty] private bool isCostError = false;
        [ObservableProperty] private bool isJobError = false;
        [ObservableProperty] private bool isSelectedRepairTypeError = false;
        [ObservableProperty] private bool isCreateNewType = false;
        [ObservableProperty] private bool isButtonCreateRepairType = true;
        [ObservableProperty] private bool isCurrentRepairType = false;
        [ObservableProperty] private bool isCancelCreateRepairType = false;

        #endregion

        public CreateRepairViewModel(
            IDialogService dialog,
            IDataService data,
            INavigationService navigate,
            RepairValidation validation,
            IPhotoPicker photoPicker,
            IPreferencesService preferencesService)
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
            _validationRepair = validation;
            _validationRepair.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
            _photoPicker = photoPicker;
            _preferencesService = preferencesService;
        }

        #region Свойства ошибок валидации

        public bool HasErrors => _validationRepair.HasErrors;
        public string MileageError => _validationRepair.GetErrors(nameof(MileageError)) as string;
        public string CostError => _validationRepair.GetErrors(nameof(CostError)) as string;
        public string JobError => _validationRepair.GetErrors(nameof(JobError)) as string;
        public string SelectedRepairTypeError => _validationRepair.GetErrors(nameof(SelectedRepairTypeError)) as string;

        #endregion

        #region Инициализация и загрузка данных

        [RelayCommand]
        public async Task Initialize(IDictionary<string, object> query)
        {
            if (_isInitialized) return;

            if (query.TryGetValue("VehicleId", out var vehicleIdObj) && vehicleIdObj is int vehicleId)
            {
                _vehicleId = vehicleId;
                _isInitialized = true;
                await LoadingCreate();
            }
            else if (query.TryGetValue("RepairId", out var repairIdObj) && repairIdObj is int repairId)
            {
                _repairId = repairId;
                _isInitialized = true;
                await LoadingUpdate();
            }

            CurrencyPicker = new ObservableCollection<string>(CurrencyList.GetListMoneySign());
            OnPropertyChanged(nameof(CurrencyPicker));

            SelectedCurrencySignPart = _preferencesService.GetDefaultMoneySign();
            SelectedCurrencySignAllCost = SelectedCurrencySignPart;
        }

        public async Task LoadingUpdate()
        {
            var result = await _dataService.GetRepairAsync(_repairId, _cts.Token);

            // Pattern matching: безопасный кастинг и проверка в одной строке
            if (result is not Result<Repair> repairResult)
            {
                StatusMessage = result.ErrorMessage;
                return;
            }

            _isUpdateRepair = true;
            ButtonName = "Редактировать";

            var data = repairResult.Data;
            SelectedRepairType = data.RepairType;
            IntervalMileageFilled = data.RepairType.IntervalMileage;
            IntervalMonthsFilled = data.RepairType.IntervalMonth;
            DateRepairSelected = data.DateRepair;
            MileageFilled = data.CurrentMileage;
            CostFilled = data.Cost;
            DescriptionFilled = data.Description;
            _vehicleId = data.VehicleId;

            if (data.SpareParts != null)
            {
                ListSpareParts.Clear();
                foreach (var part in data.SpareParts)
                    ListSpareParts.Add(part);
            }

            RepairTypes.Clear();
            RepairTypes.Add(data.RepairType);

            if (data.Photos != null)
            {
                AttachedPhotos.Clear();
                foreach (var photo in data.Photos)
                {
                    if (File.Exists(photo))
                        AttachedPhotos.Add(photo);
                }
            }

            SelectedRepairType = RepairTypes.FirstOrDefault();
        }

        public async Task LoadingCreate()
        {
            RepairTypes.Clear();

            var result = await _dataService.GetVehicleAndRepairTypesAsync(_vehicleId, _cts.Token);

            if (result is not Result<Vehicle> vehicleResult)
            {
                StatusMessage = result.ErrorMessage;
                return;
            }

            _vehicle = vehicleResult.Data;
            if (_vehicle?.RepairTypes != null)
            {
                foreach (var repairType in _vehicle.RepairTypes)
                {
                    if (repairType is not null)
                        RepairTypes.Add(repairType);
                }
            }

            MileageFilled = _vehicle?.Mileage ?? 0;
        }

        #endregion

        #region Команды управления типами ремонта

        [RelayCommand]
        public void ShowMenuCreateRepairType()
        {
            IsCreateNewType = true;
            IsButtonCreateRepairType = false;
            IsCancelCreateRepairType = true;
            SelectedRepairType = null;

            Categories.Clear(); // Важно: очищать перед добавлением, чтобы избежать дублирования
            foreach (RepairCategory cat in Enum.GetValues(typeof(RepairCategory)))
            {
                Categories.Add(cat.GetDisplay());
            }
        }

        [RelayCommand]
        public async Task CreateNewRepairType()
        {
            // Используем LINQ Any() вместо Where().Any() — быстрее
            if (RepairTypes.Any(c => c.TitleRepair == TitleNewRepairType))
            {
                await _dialogService.ShowToastAsync("Данный тип ремонта уже существует");
                return;
            }

            _newRepairType = new RepairType()
            {
                TitleRepair = TitleNewRepairType,
                IntervalMileage = IntervalMileageNewType,
                IntervalMonth = IntervalMonthNewType,
                Category = SelectedCategory.GetCategory(),
                VehicleId = _vehicleId
            };

            RepairTypes.Add(_newRepairType);
            SelectedRepairType = _newRepairType;
        }

        [RelayCommand]
        public void CancelCreateTypeRepair()
        {
            IsCreateNewType = false;
            IsButtonCreateRepairType = true;
            IsCancelCreateRepairType = false;
        }

        #endregion

        #region Главная команда: создание/редактирование ремонта

        [RelayCommand]
        public async Task ProcessingRepair()
        {
            ValidationAll();
            if (HasErrors) return;

            // Шаг 1: Создаём новый тип ремонта, если он был создан пользователем
            if (SelectedRepairType == _newRepairType)
            {
                if (!await TryCreateNewRepairTypeAsync())
                    return;
            }

            // Шаг 2: Создаём объект ремонта
            _repair = BuildRepairEntity();

            // Шаг 3: Сохраняем фото, если они есть
            if (AttachedPhotos?.Any() == true)
            {
                await TrySavePhotosAsync(_repair);
            }

            // Шаг 4: Создаём или обновляем ремонт в базе
            if (!await TrySaveRepairAsync(_repair))
                return;

            // Шаг 5: Обновляем пробег авто и интервалы обслуживания
            if (!await TryUpdateRepairRelatedDataAsync())
                return;

            // Шаг 6: Возвращаемся назад, если всё успешно
            if (string.IsNullOrWhiteSpace(StatusMessage))
            {
                await _navigationService.GoToBack();
            }
        }

        #endregion

        #region Команды работы с фото

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

        #region Команды работы с запчастями

        [RelayCommand]
        public void CreateSparePart()
        {
            var sparePart = new SparePart()
            {
                NamePart = CreateNamePart,
                ArticleNumberPart = CreateArticleNumberPart,
                CostPart = CreateCostPart,
                CurrentCurrencySing = SelectedCurrencySignPart
            };

            ListSpareParts.Add(sparePart);

            // Очищаем поля ввода
            CreateNamePart = string.Empty;
            CreateArticleNumberPart = string.Empty;
            CreateCostPart = 0;
        }

        [RelayCommand]
        public async Task ShowOptions(SparePart part)
        {
            var result = await _dialogService.ShowDisplayAction();

            switch (result)
            {
                case "Удалить":
                    ListSpareParts.Remove(part);
                    break;
                case "Редактировать":
                    CreateNamePart = part.NamePart;
                    CreateArticleNumberPart = part.ArticleNumberPart;
                    CreateCostPart = part.CostPart;
                    ListSpareParts.Remove(part);
                    break;
            }
        }

        #endregion

        #region Приватные методы бизнес-логики

        private void ValidationAll()
        {
            IsMileageError = _validationRepair.ValidationMileage(MileageFilled);
            IsCostError = _validationRepair.ValidationCost(CostFilled);
            IsJobError = _validationRepair.ValidationJob(SelectedJob);
            IsSelectedRepairTypeError = _validationRepair.ValidationSelectedRepairType(SelectedRepairType);
        }

        /// <summary>
        /// Добавляет сообщение об ошибке к StatusMessage, избегая дублирования разделителей.
        /// Применяется везде, где возвращается ошибка от сервисов.
        /// </summary>
        private void AppendError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                return;

            if (string.IsNullOrWhiteSpace(StatusMessage))
                StatusMessage = errorMessage;
            else
                StatusMessage += " " + errorMessage;
        }

        /// <summary>
        /// Пытается создать новый тип ремонта в базе.
        /// Возвращает true при успехе, false при ошибке.
        /// </summary>
        private async Task<bool> TryCreateNewRepairTypeAsync()
        {
            var result = await _dataService.CreateRepairTypeAsync(_newRepairType, _cts.Token);
            if (!result.Success)
            {
                AppendError(result.ErrorMessage);
                return false;
            }

            // Получаем созданный тип с актуальным Id
            var getResult = await _dataService.GetRepairTypeAsync(_newRepairType, _cts.Token);
            if (getResult is Result<RepairType> typedResult)
            {
                _newRepairType = typedResult.Data;
            }

            return true;
        }

        /// <summary>
        /// Сохраняет фото и присваивает пути к объекту ремонта.
        /// </summary>
        private async Task TrySavePhotosAsync(Repair repair)
        {
            var result = await _photoPicker.SavePhotosAsync(AttachedPhotos, _cts.Token);
            if (result.Success && result is Result<List<string>> typedResult)
            {
                repair.Photos = typedResult.Data;
            }
        }

        /// <summary>
        /// Создаёт или обновляет ремонт в базе в зависимости от режима.
        /// Возвращает true при успехе, false при ошибке.
        /// </summary>
        private async Task<bool> TrySaveRepairAsync(Repair repair)
        {
            Result result;

            if (_isUpdateRepair)
            {
                repair.Id = _repairId;
                result = await _dataService.UpdateRepairAsync(repair, _cts.Token);
            }
            else
            {
                result = await _dataService.CreateRepairAsync(repair, _cts.Token);
            }

            if (!result.Success)
            {
                AppendError(result.ErrorMessage);
                return false;
            }

            if (_isUpdateRepair)
                _isUpdateRepair = false;

            return true;
        }

        /// <summary>
        /// Обновляет пробег автомобиля и интервалы обслуживания типа ремонта.
        /// Возвращает false при критических ошибках (например, не удалось сохранить тип).
        /// </summary>
        private async Task<bool> TryUpdateRepairRelatedDataAsync()
        {
            if (SelectedRepairType is null)
                return false;

            SelectedRepairType.LastServiceMileage = MileageFilled;
            SelectedRepairType.LastServiceDate = DateRepairSelected;
            SelectedRepairType.IntervalMileage = IntervalMileageFilled;
            SelectedRepairType.IntervalMonth = IntervalMonthsFilled;

            var vehicleResult = await _dataService.GetVehicleMileageAsync(_vehicleId, _cts.Token);
            if (vehicleResult is Result<Vehicle> vehicleTypedResult && vehicleTypedResult.Data is not null)
            {
                _vehicle = vehicleTypedResult.Data;

                // ✅ ТЕПЕРЬ БЕЗОПАСНО: _vehicle точно не null
                if (MileageFilled > _vehicle.Mileage)
                {
                    var mileageResult = await _dataService.UpdateVehicleMileageAsync(_vehicleId, MileageFilled, _cts.Token);
                    if (!mileageResult.Success)
                        AppendError(mileageResult.ErrorMessage);
                }
            }

            var typeResult = await _dataService.UpdateRepairTypeAsync(SelectedRepairType, _cts.Token);
            if (!typeResult.Success)
            {
                AppendError(typeResult.ErrorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Создаёт объект Repair на основе текущих данных формы.
        /// </summary>
        private Repair BuildRepairEntity()
        {
            var repair = new Repair()
            {
                DateRepair = DateRepairSelected,
                Cost = CostFilled,
                Description = DescriptionFilled,
                VehicleId = _vehicleId,
                RepairTypeId = SelectedRepairType.Id,
                CurrentMileage = MileageFilled,
                CommentMechanic = CommentMechanic,
                ServiceName = ServiceName,
                Job = SelectedJob,
            };

            if (ListSpareParts.Any())
                repair.SpareParts = ListSpareParts.ToList();

            return repair;
        }

        #endregion

        #region Частичные методы для ObservableProperty (генерируются Source Generator'ом)

        partial void OnSelectedJobChanged(string value)
        {
            isJobError = _validationRepair.ValidationJob(value);
        }

        partial void OnSelectedRepairTypeChanged(RepairType value)
        {
            if (value == null)
            {
                IsCurrentRepairType = false;
                IsButtonCreateRepairType = false;
                IsCreateNewType = true;
                return;
            }

            IsCurrentRepairType = true;
            IsButtonCreateRepairType = true;
            IsCreateNewType = false;
            IntervalMileageFilled = value.IntervalMileage;
            IntervalMonthsFilled = value.IntervalMonth;
        }

        partial void OnMileageFilledChanged(double value)
        {
            IsMileageError = _validationRepair.ValidationMileage(value);
        }

        partial void OnCostFilledChanged(decimal value)
        {
            IsCostError = _validationRepair.ValidationCost(value);
        }

        void OnErrorsChangedUI(DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(e.PropertyName);
        }

        #endregion

        #region Управление токеном отмены и событиями

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }

        [RelayCommand]
        public void OffEvent()
        {
            // TODO: Эта отписка не работает из-за нового анонимного делегата.
            // Нужно сохранить EventHandler в поле, чтобы можно было отписаться корректно.
            _validationRepair.ErrorsChanged -= (s, e) => OnErrorsChangedUI(e);
        }

        #endregion
    }
}