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


namespace AutoCareDiray.Shared.ViewModels.RepairViewModel
{
    public partial class CreateRepairViewModel : BaseViewModel
    {
        #region основные классы и списки

        private CancellationTokenSource _cts;
        private RepairValidation _validationRepair;
        private IPhotoPicker _photoPicker;

        private Repair Repair;
        private int _vehicleId = -1;
        private int _repairId = -1;
        private bool isInitialize = false;
        private bool _isUpdateRepair = false;
        private Vehicle _vehicle;
        private string _job = String.Empty;

        #endregion

        #region классы и свойства для работы UI

        public ObservableCollection<RepairType> RepairTypes { get; set; } = new ObservableCollection<RepairType>();

        [ObservableProperty]
        private RepairType selectedRepairType;
        [ObservableProperty]
        private int intervalMileageFilled;
        [ObservableProperty]
        private int intervalMonthsFilled;
        [ObservableProperty]
        private DateTime dateRepairSelected = DateTime.UtcNow;
        [ObservableProperty]
        private int mileageFilled;
        [ObservableProperty]
        private string sparePartsFilled;
        [ObservableProperty]
        private string costFilled;
        [ObservableProperty]
        private string descriptionFilled;
        [ObservableProperty]
        private string statusMessage;
        [ObservableProperty]
        private string buttonName = "Создать";

        [ObservableProperty]
        private bool isMileageError = false;
        [ObservableProperty]
        private bool isCostError = false;
        [ObservableProperty]
        private string selectedJob;
        [ObservableProperty]
        private string serviceName;
        [ObservableProperty]
        private string commentMechanic;

        [ObservableProperty]
        ObservableCollection<string> attachedPhotosRepairs;

        #endregion

        public CreateRepairViewModel(IDialogService dialog,IDataService data,INavigationService navigate,RepairValidation validation,IPhotoPicker photoPicker) 
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
            _validationRepair = validation;
            _validationRepair.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
            _photoPicker = photoPicker;
        }

        #region свойства для ошибок в реальном времени

        public bool HasErrors => _validationRepair.HasErrors;
        public string MileageError => _validationRepair.GetErrors(nameof(MileageError)) as String;
        public string CostError => _validationRepair.GetErrors(nameof(CostError)) as String;

        #endregion

        Func<string, int> ConverFromInt = (property) =>
        {
            if (int.TryParse(property, out int result))
            {
                return result;
            }
            else return 0;
        }; //конвертирует со string in Int

        [RelayCommand]
        public async Task Initialize(IDictionary<string,object> query)
        {
            if (isInitialize) return;
            if(query.TryGetValue("VehicleId",out var obj))
            {
                if(obj is int value)
                {
                    _vehicleId = value;
                    isInitialize = true;
                    await LoadingCreate();
                }
            }
            else if(query.TryGetValue("RepairId", out var objTwo))
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
                SparePartsFilled = resultRepair.Data.SpareParts;
                CostFilled = resultRepair.Data.Cost.ToString();
                DescriptionFilled = resultRepair.Data.Description;
                _vehicleId = resultRepair.Data.VehicleId;

                if (RepairTypes.Count > 0) RepairTypes.Clear();
                RepairTypes.Add(resultRepair.Data.RepairType);

                if (resultRepair.Data.Photos != null)
                {
                    AttachedPhotosRepairs ??= new ObservableCollection<string>();
                    foreach (var photo in resultRepair.Data.Photos)
                    {
                        if (File.Exists(photo)) AttachedPhotosRepairs.Add(photo);
                    }
                }
                SelectedRepairType = RepairTypes.FirstOrDefault();
            }
            else StatusMessage = result.ErrorMessage;
        } //Загрузка ресурсов под редактирования 

        public async Task LoadingCreate()
        {
            if(RepairTypes.Count > 0) RepairTypes.Clear();
            var result = await _dataService.GetVehicleAndRepairTypesAsync(_vehicleId, _cts.Token);
            if (result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                _vehicle = resultVehicle.Data;
                foreach (var repairs in _vehicle.RepairTypes)
                {
                    if (repairs is not null) RepairTypes.Add(repairs);
                }
                SelectedRepairType = RepairTypes.FirstOrDefault(new RepairType());
                IntervalMileageFilled = SelectedRepairType.IntervalMileage;
                IntervalMonthsFilled = SelectedRepairType.IntervalMonth;
                MileageFilled = _vehicle.Mileage;
            }
            else StatusMessage = result.ErrorMessage;
        } //Загрузка под создания ремонта

        [RelayCommand]
        public async Task ProcessingRepair()
        {
            _validationRepair.ValidationAll(MileageFilled, CostFilled);
            if(HasErrors) return;
            Repair = CreateRepai();

            var result = await _photoPicker.SavePhotosAsync(AttachedPhotosRepairs, _cts.Token);
            if (result.Success)
            {
                var resultPhotos = result as Result<List<string>>;
                Repair.Photos = resultPhotos.Data;
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
            await _navigationService.GoToBack();
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
            AttachedPhotosRepairs ??= new();

            foreach (var listPhoto in resultPhoto.Data)
                {
                    AttachedPhotosRepairs.Add(listPhoto);
                }

        } //выбор фото
        [RelayCommand]
        public async Task DeleteAttachPhoto(string photo)
        {
            AttachedPhotosRepairs?.Remove(photo);
        } //удаление фото

        private Repair CreateRepai()
        {
            Repair repair = new Repair()
            {
                DateRepair = DateRepairSelected,
                SpareParts = SparePartsFilled,
                Cost = ConverFromInt(CostFilled),
                Description = DescriptionFilled,
                VehicleId = _vehicleId,
                RepairTypeId = SelectedRepairType.Id,
                CurrentMileage = MileageFilled,
                CommentMechanic = CommentMechanic,
                ServiceName = ServiceName,
                Job = SelectedJob,
            };

            return repair;
        } // создание ремонита
        private async Task UpdateDate()
        {

            SelectedRepairType.LastServiceMileage = MileageFilled;
            SelectedRepairType.LastServiceDate = DateRepairSelected;

            var result = await _dataService.GetVehicleMileageAsync(_vehicleId,_cts.Token);
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
       

        partial void OnSelectedRepairTypeChanged(RepairType value)
        {
            IntervalMileageFilled = value.IntervalMileage;
        }
        partial void OnMileageFilledChanged(int value)
        {
            _validationRepair.ValidationMileage(value);
        }
        partial void OnCostFilledChanged(string value)
        {
            _validationRepair.ValidationCost(value);
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
