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
            var result = await _dataService.GetRepairAsync(_repairId, _cts.Token);
            if (result.Success)
            {
                var resultRepair = result as Result<Repair>;
                _isUpdateRepair = true;
                ButtonName = " Редактировать";

                SelectedRepairType = resultRepair.Data.RepairType;
                IntervalMileageFilled = resultRepair.Data.RepairType.IntervalMileage;
                DateRepairSelected = resultRepair.Data.DateRepair;
                MileageFilled = resultRepair.Data.CurrentMileage;
                SparePartsFilled = resultRepair.Data.SpareParts;
                CostFilled = resultRepair.Data.Cost.ToString();
                DescriptionFilled = resultRepair.Data.Description;

                if (RepairTypes.Count > 0) RepairTypes.Clear();
                RepairTypes.Add(resultRepair.Data.RepairType);
                SelectedRepairType = RepairTypes.FirstOrDefault(new RepairType());
            }
            else StatusMessage = result.ErrorMessage;
        }

        //Загрузка под создания ремонта
        public async Task Loading()
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
                MileageFilled = _vehicle.Mileage;
            }
            else StatusMessage = result.ErrorMessage;
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

            if (MileageFilled > _vehicle.Mileage)
            {
               var resultMileage =  await _dataService.UpdateVehicleMileageAsync(_vehicleId, MileageFilled, _cts.Token);
               if (resultMileage.Success == false)
                {
                    StatusMessage = resultMileage.ErrorMessage;
                    return;
                }
            }

            if (_isUpdateRepair)
            {
                var resultUdateRepair = await _dataService.UpdateRepairAsync(repairCreate, _cts.Token);
                if(resultUdateRepair.Success == false)
                {
                    StatusMessage = resultUdateRepair.ErrorMessage;
                    return;
                }


                if (SelectedRepairType.IntervalMileage != IntervalMileageFilled)
                {
                    SelectedRepairType.IntervalMileage = IntervalMileageFilled;
                    var resultUpdateTypeRep = await _dataService.UpdateRepairTypeAsync(SelectedRepairType, _cts.Token);
                    if (resultUpdateTypeRep.Success == false)
                    {
                        StatusMessage = resultUpdateTypeRep.ErrorMessage;
                        return;
                    }
                }

                _isUpdateRepair = false;
                if (resultUdateRepair.Success) await _navigationService.GoToBack();
                else StatusMessage = resultUdateRepair.ErrorMessage;
            }
            else
            {
                var resultCreateRepair = await _dataService.CreateRepairAsync(repairCreate, _cts.Token);
                if(resultCreateRepair.Success == false)
                {
                    StatusMessage = resultCreateRepair.ErrorMessage;
                    return;
                }

                if (SelectedRepairType.IntervalMileage != IntervalMileageFilled)
                {
                    SelectedRepairType.IntervalMileage = IntervalMileageFilled;
                    var resultUpdate = await _dataService.UpdateRepairTypeAsync(SelectedRepairType, _cts.Token);
                    if (resultUpdate.Success == false)
                    {
                        StatusMessage = resultUpdate.ErrorMessage;
                        return;
                    }
                }

                if (resultCreateRepair.Success) await _navigationService.GoToBack();
                else StatusMessage = resultCreateRepair.ErrorMessage;
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
