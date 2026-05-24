using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.Validation;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.ValidationService;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.Shared.Service.ResultService;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;


namespace AutoCareDiray.Shared.ViewModels.RepairViewModel
{
    public partial class CreateRepairViewModel : BaseViewModel
    {
        #region основные классы и списки

        private CancellationTokenSource _cts;
        private readonly RepairValidation _validationRepair;
        private readonly IPhotoPicker _photoPicker;
        private readonly IPreferencesService _preferencesService;

        private Repair Repair;
        private Vehicle _vehicle;
        private RepairType _type;

        private int _vehicleId = -1;
        private int _repairId = -1;

        private bool isInitialize = false;
        private bool _isUpdateRepair = false;


        private string _job = String.Empty;

        #endregion

        #region классы и свойства для работы UI

        public ObservableCollection<RepairType> RepairTypes { get; set; } = new ObservableCollection<RepairType>();
        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> AttachedPhotos { get; set; }
        public ObservableCollection<SparePart> ListSpareParts { get; set; } = new();


        [ObservableProperty]
        private RepairType selectedRepairType;

        [ObservableProperty]
        private int intervalMileageFilled;
        [ObservableProperty]
        private int intervalMonthsFilled;
        [ObservableProperty]
        private int mileageFilled;
        [ObservableProperty]
        private decimal costFilled;
        [ObservableProperty]
        private decimal createCostPart;
        [ObservableProperty]
        private int intervalMileageNewType;
        [ObservableProperty]
        private int intervalMonthNewType;

        [ObservableProperty]
        private DateTime dateRepairSelected = DateTime.UtcNow;

        [ObservableProperty]
        private string selectedSparePart;
        [ObservableProperty]
        private string descriptionFilled;
        [ObservableProperty]
        private string statusMessage;
        [ObservableProperty]
        private string buttonName = "Создать";
        [ObservableProperty]
        private string selectedJob;
        [ObservableProperty]
        private string serviceName;
        [ObservableProperty]
        private string commentMechanic;
        [ObservableProperty]
        private string createNamePart;
        [ObservableProperty]
        private string createArticleNumberPart;
        [ObservableProperty]
        private string selectedCategory;
        [ObservableProperty]
        private string titleNewRepairType;



        [ObservableProperty]
        private bool isMileageError = false;
        [ObservableProperty]
        private bool isCostError = false;
        [ObservableProperty]
        private bool isJobError = false;
        [ObservableProperty]
        private bool isSelectedRepairTypeError = false;

        public string IntervalMileageText => "Интервал пробега " + _preferencesService.GetDefaultShortDistance();
        public string InvervalMileageNewText => "Введите Интервал пробега или оставте 0" + _preferencesService.GetDefaultShortDistance();
        public string MileageText => "Текущий пробег авто " + _preferencesService.GetDefaultShortDistance();
        public string CostText => "Общая стоимость " + _preferencesService.GetDefaultMoney();
        public string CostPartText => "Стоимость запчасти " + _preferencesService.GetDefaultMoney();

        #endregion

        public CreateRepairViewModel(IDialogService dialog, IDataService data, INavigationService navigate, RepairValidation validation, IPhotoPicker photoPicker, IPreferencesService preferencesService)
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
            _validationRepair = validation;
            _validationRepair.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
            _photoPicker = photoPicker;
            _preferencesService = preferencesService;
        }

        #region свойства для ошибок в реальном времени

        public bool HasErrors => _validationRepair.HasErrors;
        public string MileageError => _validationRepair.GetErrors(nameof(MileageError)) as String;
        public string CostError => _validationRepair.GetErrors(nameof(CostError)) as String;
        public string JobError => _validationRepair.GetErrors(nameof(JobError)) as String;
        public string SelectedRepairTypeError => _validationRepair.GetErrors(nameof(SelectedRepairTypeError)) as String;

        #endregion

        [RelayCommand]
        public async Task Initialize(IDictionary<string, object> query)
        {
            if (isInitialize) return;
            if (query.TryGetValue("VehicleId", out var obj))
            {
                if (obj is int value)
                {
                    _vehicleId = value;
                    isInitialize = true;
                    await LoadingCreate();
                }
            }
            else if (query.TryGetValue("RepairId", out var objTwo))
            {
                if (objTwo is int value)
                {
                    _repairId = value;
                    isInitialize = true;
                    await LoadingUpdate();
                }
            }
        } //Инициализация и определения редактирования или обновления данных

        public async Task LoadingUpdate()
        {
            var result = await _dataService.GetRepairAsync(_repairId, _cts.Token);
            if (result.Success)
            {
                var resultRepair = result as Result<Repair>;
                _isUpdateRepair = true;
                ButtonName = " Редактировать";

                SelectedRepairType = resultRepair.Data.RepairType;
                IntervalMileageFilled = resultRepair.Data.RepairType.IntervalMileage;
                IntervalMonthsFilled = resultRepair.Data.RepairType.IntervalMonth;
                DateRepairSelected = resultRepair.Data.DateRepair;
                MileageFilled = resultRepair.Data.CurrentMileage;
                CostFilled = resultRepair.Data.Cost;
                DescriptionFilled = resultRepair.Data.Description;
                _vehicleId = resultRepair.Data.VehicleId;

                if (resultRepair.Data.SpareParts != null)
                {
                    foreach (var part in resultRepair.Data.SpareParts)
                    {
                        ListSpareParts.Add(part);
                    }
                }

                if (RepairTypes.Count > 0) RepairTypes.Clear();
                RepairTypes.Add(resultRepair.Data.RepairType);

                if (resultRepair.Data.Photos != null)
                {
                    AttachedPhotos ??= new ObservableCollection<string>();
                    foreach (var photo in resultRepair.Data.Photos)
                    {
                        if (File.Exists(photo)) AttachedPhotos.Add(photo);
                    }
                }
                SelectedRepairType = RepairTypes.FirstOrDefault();
            }
            else StatusMessage = result.ErrorMessage;
        } //Загрузка ресурсов под редактирования 

        public async Task LoadingCreate()
        {
            if (RepairTypes.Count > 0) RepairTypes.Clear();

            var result = await _dataService.GetVehicleAndRepairTypesAsync(_vehicleId, _cts.Token);
            if (result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                _vehicle = resultVehicle.Data;
                if(_vehicle.RepairTypes.Count > 0)
                {
                    foreach (var repairs in _vehicle.RepairTypes)
                    {
                        if (repairs is not null) RepairTypes.Add(repairs);
                    }
                    SelectedRepairType = RepairTypes.First();
                    IntervalMileageFilled = SelectedRepairType.IntervalMileage;
                    IntervalMonthsFilled = SelectedRepairType.IntervalMonth;
                }
                MileageFilled = _vehicle.Mileage;
            }
            else StatusMessage = result.ErrorMessage;
        } //Загрузка под создания ремонта

        [RelayCommand]
        public void GetCategoriesRepairType(List<string> categories)
        {
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        [RelayCommand]
        public async Task CreateNewRepairType()
        {
            if (RepairTypes.Where(c => c.TitleRepair == TitleNewRepairType).Any())
            {
                await _dialogService.ShowToastAsync("Данный тип ремонта уже сеществует");
                return;
            }
            _type = new RepairType()
            {
                TitleRepair = TitleNewRepairType,
                IntervalMileage = IntervalMileageNewType,
                IntervalMonth = IntervalMonthNewType,
                Category = SelectedCategory,
                VehicleId = _vehicleId
            };

            RepairTypes.Add(_type);
            SelectedRepairType = _type;
        }

        [RelayCommand]
        public async Task ProcessingRepair()
        {
            ValidationAll();
            if (HasErrors) return;

            if (SelectedRepairType == _type)
            {
                var resultUpdateRepairType = await _dataService.CreateRepairTypeAsync(_type, _cts.Token);
                if (resultUpdateRepairType.Success == false)
                {
                    StatusMessage = string.IsNullOrWhiteSpace(StatusMessage) == false
                       ? StatusMessage += " " + resultUpdateRepairType.ErrorMessage
                       : StatusMessage = resultUpdateRepairType.ErrorMessage;
                    return;
                }
                else
                {
                    var result = await _dataService.GetRepairTypeAsync(_type, _cts.Token);
                    var resultRepairType = result as Result<RepairType>;
                    _type = resultRepairType.Data;
                }
            }

            Repair = CreateRepair();

            if (AttachedPhotos != null)
            {
                var result = await _photoPicker.SavePhotosAsync(AttachedPhotos, _cts.Token);
                if (result.Success)
                {
                    var resultPhotos = result as Result<List<string>>;
                    Repair.Photos = resultPhotos.Data;
                }
            }

            if (_isUpdateRepair)
            {
                Repair.Id = _repairId;
                var resultUdateRepair = await _dataService.UpdateRepairAsync(Repair, _cts.Token);
                if (resultUdateRepair.Success == false)
                {
                    StatusMessage = resultUdateRepair.ErrorMessage;
                    return;
                }
                _isUpdateRepair = false;
            }
            else
            {
                var resultCreateRepair = await _dataService.CreateRepairAsync(Repair, _cts.Token);
                if (resultCreateRepair.Success == false)
                {
                    StatusMessage = resultCreateRepair.ErrorMessage;
                    return;
                }
            }

            await UpdateDate();
            if (string.IsNullOrWhiteSpace(StatusMessage))
            {
                await _navigationService.GoToBack();
            }
        }  //Создание или редактирование ремонта

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
        public void DeleteAttachPhoto(string photo)
        {
            AttachedPhotos?.Remove(photo);
        } //удаление фото

        private Repair CreateRepair()
        {

            Repair repair = new Repair()
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

            if (ListSpareParts.Count > 0)
            {
                repair.SpareParts = ListSpareParts.ToList();
            }

            return repair;
        } // создание ремонита
        private async Task UpdateDate()
        {

            SelectedRepairType.LastServiceMileage = MileageFilled;
            SelectedRepairType.LastServiceDate = DateRepairSelected;

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

            if (SelectedRepairType.IntervalMileage != IntervalMileageFilled || SelectedRepairType.IntervalMonth != IntervalMonthsFilled)
            {
                SelectedRepairType.IntervalMileage = IntervalMileageFilled;
                SelectedRepairType.IntervalMonth = IntervalMonthsFilled;
                var resultUpdateTypeRep = await _dataService.UpdateRepairTypeAsync(SelectedRepairType, _cts.Token);
                if (resultUpdateTypeRep.Success == false)
                {
                    if (string.IsNullOrWhiteSpace(StatusMessage) == false)
                    {
                        StatusMessage += " " + resultUpdateTypeRep.ErrorMessage;
                    }
                    else
                    {
                        StatusMessage = resultUpdateTypeRep.ErrorMessage;
                    }
                }
            }

        } // Обновление пробега у авто и интервала пробега

        [RelayCommand]
        public void CreateSparePart()
        {
            var sparePart = new SparePart()
            {
                NamePart = CreateNamePart,
                ArticleNumberPart = CreateArticleNumberPart,
                CostPart = CreateCostPart
            };

            ListSpareParts.Add(sparePart);
            CreateNamePart = "";
            CreateArticleNumberPart = "";
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
                    break;
            }
        }


        private void ValidationAll()
        {
            IsMileageError = _validationRepair.ValidationMileage(MileageFilled);
            IsCostError = _validationRepair.ValidationCost(CostFilled);
            IsJobError = _validationRepair.ValidationJob(SelectedJob);
            IsSelectedRepairTypeError = _validationRepair.ValidationSelectedRepairType(SelectedRepairType);
        }


        partial void OnSelectedJobChanged(string value)
        {
            isJobError = _validationRepair.ValidationJob(value);
        }
        partial void OnSelectedRepairTypeChanged(RepairType value)
        {
            IntervalMileageFilled = value.IntervalMileage;
            IntervalMonthsFilled = value.IntervalMonth;
        }
        partial void OnMileageFilledChanged(int value)
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



        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }  //отмена токена

        [RelayCommand]
        public void OffEvent()
        {
            _validationRepair.ErrorsChanged -= (s, e) => OnErrorsChangedUI(e);
        }  //отмена подписки на событие
    }
}
