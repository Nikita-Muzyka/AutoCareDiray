using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.Validation;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.ValidationService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace AutoCareDiray.Shared.ViewModels.RepairViewModel
{
    public partial class CreateRepairViewModel : BaseViewModel
    {
        CancellationTokenSource _cts;
        RepairValidation _validationRepair;

        private int _vehicleId = -1;
        private int _repairId = -1;
        private bool isInitialize = false;
        private bool _isUpdateRepair = false;
        private Vehicle _vehicle;

        [ObservableProperty]
        private string buttonName = "Создать";
        public ObservableCollection<RepairType> RepairTypes { get; set; } = new ObservableCollection<RepairType>();

        [ObservableProperty]
        private RepairType selectedRepairType;
        [ObservableProperty]
        private int intervalMileageFilled;
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

        public CreateRepairViewModel(IDialogService dialog,IDataService data,INavigationService navigate,RepairValidation validation) 
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
            _validationRepair = validation;
            _validationRepair.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
        }

        public bool HasErrors => _validationRepair.HasErrors;
        public string MileageError => _validationRepair.GetErrors(nameof(MileageError)) as String;
        public string CostError => _validationRepair.GetErrors(nameof(CostError)) as String;

        Func<string, int> ConverFromInt = (property) =>
        {
            if (int.TryParse(property, out int result))
            {
                return result;
            }
            else return 0;
        };


        //Инициализация и определения редактирования или обновления данных
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
                    await Loading();
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
        }

        //Загрузка ресурсов под редактирования 
        public async Task LoadingUpdate()
        {
            var repairUpdate = await _dataService.GetRepairAsync(_repairId, _cts.Token);
            if (repairUpdate != null)
            {
                _isUpdateRepair = true;
                ButtonName = " Редактировать";

                SelectedRepairType = repairUpdate.RepairType;
                IntervalMileageFilled = repairUpdate.RepairType.IntervalMileagee;
                DateRepairSelected = repairUpdate.DateRepair;
                MileageFilled = repairUpdate.CurrentMileage;
                SparePartsFilled = repairUpdate.SpareParts;
                CostFilled = repairUpdate.Cost.ToString();
                DescriptionFilled = repairUpdate.Description;

                if (RepairTypes.Count > 0) RepairTypes.Clear();
                RepairTypes.Add(repairUpdate.RepairType);
                SelectedRepairType = RepairTypes.FirstOrDefault(new RepairType());
            }
            else await _dialogService.ShowToastAsync("Ремонт не загрузился");
        }

        //Загрузка под создания ремонта
        public async Task Loading()
        {
            if(RepairTypes.Count > 0) RepairTypes.Clear();
            _vehicle = await _dataService.GetVehicleAndRepairTypesAsync(_vehicleId, _cts.Token);
            if(_vehicle != null)
            {
                foreach (var repairs in _vehicle.ReepairTypes)
                {
                    if (repairs is not null) RepairTypes.Add(repairs);
                }
                SelectedRepairType = RepairTypes.FirstOrDefault(new RepairType());
                IntervalMileageFilled = SelectedRepairType.IntervalMileagee;
                MileageFilled = _vehicle.Mileage;
            }
        }


        //Создание или редактирование ремонта
        [RelayCommand]
        public async Task CreateRepair()
        {
            _validationRepair.ValidationAll(MileageFilled, CostFilled);
            if(HasErrors) return;
            Repair repairCreate = new Repair()
            {
                DateRepair = DateRepairSelected,
                SpareParts = SparePartsFilled,
                Cost = ConverFromInt(CostFilled),
                Description = DescriptionFilled,
                VehicleId = _vehicleId,
                RepairTypeId = SelectedRepairType.Id,
                CurrentMileage = MileageFilled,
            };

            if (MileageFilled > _vehicle.Mileage) await _dataService.UpdateVehicleMileageAsync(_vehicleId, MileageFilled, _cts.Token);

            if (_isUpdateRepair)
            {
                var repairSuccess = await _dataService.UpdateRepairAsync(repairCreate, _cts.Token);
                if (SelectedRepairType.IntervalMileagee != IntervalMileageFilled)
                {
                    SelectedRepairType.IntervalMileagee = IntervalMileageFilled;
                    await _dataService.UpdateRepairTypeAsync(SelectedRepairType, _cts.Token);
                }
                _isUpdateRepair = false;
                if (repairSuccess) await _navigationService.GoToBack();
                else StatusMessage = "Ремонт не был сохранен";
            }
            else
            {
                var repairSuccess = await _dataService.CreateRepairAsync(repairCreate, _cts.Token);

                if (SelectedRepairType.IntervalMileagee != IntervalMileageFilled)
                {
                    SelectedRepairType.IntervalMileagee = IntervalMileageFilled;
                    await _dataService.UpdateRepairTypeAsync(SelectedRepairType, _cts.Token);
                }

                if (repairSuccess) await _navigationService.GoToBack();
                else StatusMessage = "Ремонт не был сохранен";
            }
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
        }
    }
}
