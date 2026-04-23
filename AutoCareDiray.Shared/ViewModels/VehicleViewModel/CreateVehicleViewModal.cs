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


namespace AutoCareDiray.Shared.ViewModels.VehicleViewModel
{
    public partial class CreateVehicleViewModel : BaseViewModel
    {
        #region Основные поля для работы

        private VehicleValidation _vehicleValidation;
        private IPhotoPicker _photoPicker;
        private CancellationTokenSource _cts;
        private Vehicle _vehicle;
        private List<RepairType> _listRepairType;
        //Инициализирована ли страница
        private bool _isInitilized = false;
        //переключатель с создания авто на обновление данных авто
        private bool _isUpdateVehicle = false;
        #endregion


        #region Свойства для работы UI
        [ObservableProperty]
        private ObservableCollection<RepairGroup> repairGrouped;
        [ObservableProperty]
        private RepairGroup selectedGroup;

        [ObservableProperty]
        private bool isNameVehicle = false;
        [ObservableProperty]
        private bool isYearPurchaseError = false;
        [ObservableProperty]
        private bool isVinCode = false;
        [ObservableProperty]
        private bool isStateNumber = false;
        [ObservableProperty]
        private bool isMileageError = false;
        [ObservableProperty]
        private bool isMileage = false;
        [ObservableProperty]
        private bool isTypeVehicleError = false;
        [ObservableProperty]
        private string buttonName = "Создать";

        [ObservableProperty]
        private string nameVehicle = String.Empty;
        [ObservableProperty]
        private DateTime yearCreateSelected = DateTime.Today;
        [ObservableProperty]
        private string mileage = String.Empty;
        [ObservableProperty]
        private string vinCode = String.Empty;
        [ObservableProperty]
        private string stateNumber = String.Empty;
        [ObservableProperty]
        private DateTime yearPurchaseSelected = DateTime.Today;
        [ObservableProperty]
        private string selectedTypeVehicle = String.Empty;
        [ObservableProperty]
        private DateTime dateNow = DateTime.Today;
        [ObservableProperty]
        private string transmissionType = String.Empty;
        [ObservableProperty]
        private string pathPhoto = "car_add_icon.png";
        [ObservableProperty]
        private string statusMessage;

        private string _photoVehicle = String.Empty;
        #endregion


        public CreateVehicleViewModel(IDialogService dialogService,IDataService dataService,INavigationService navigation,IPhotoPicker photoPicker,
            VehicleValidation vehicleValidation) : base(dialogService,dataService,navigation)
        {
            _vehicleValidation = vehicleValidation;
            _photoPicker = photoPicker;
            _vehicleValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
            _cts = new CancellationTokenSource();
        }


        #region получение текса ошибок
        public bool HasErrors => _vehicleValidation.HasErrors;
        public string MileageError => _vehicleValidation.GetErrors(nameof(MileageError)) as string ?? String.Empty;
        public string YearPurchaseError => _vehicleValidation.GetErrors(nameof(YearPurchaseError)) as string ?? String.Empty;
        public string TypeVehicleError => _vehicleValidation.GetErrors(nameof(TypeVehicleError)) as string ?? String.Empty;
        #endregion


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

            var result = await _dataService.GetVehicleAndRepairTypesForUpdateAsync(VehicleId, _cts.Token);
            if (result.Success)
            {
                var resultVehicle = result as Result<Vehicle>;
                _vehicle = resultVehicle.Data ?? new Vehicle();
                var groups = _vehicle.RepairTypes
                    .GroupBy(g => g.Category)
                    .Select(g => new RepairGroup(g.Key,g.ToList()))
                    .ToList();
                RepairGrouped = new ObservableCollection<RepairGroup>(groups);
                _listRepairType = RepairGrouped.SelectMany(c => c).ToList();

                _isUpdateVehicle = true;
                _isInitilized = true;
                ButtonName = "Изменить";

                if (System.IO.File.Exists(_vehicle.PhotoVehicle)) PathPhoto = _vehicle.PhotoVehicle;
                NameVehicle = _vehicle.NameVehicle;
                YearCreateSelected = _vehicle.YearCreate;
                YearPurchaseSelected = _vehicle.YearPurchase;
                Mileage = _vehicle.Mileage.ToString();
                SelectedTypeVehicle = _vehicle.VehicleType;
                VinCode = _vehicle.VinCode;
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

            var groups = CreateRepairTypeGroups();

            RepairGrouped = new ObservableCollection<RepairGroup>(groups);

            _listRepairType = RepairGrouped.SelectMany(c => c).ToList();
            _isInitilized = true;
        }

        /// <summary>
        /// Создание авто
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task CreateVehicle()
        {
            _vehicleValidation.ValidationAll(Mileage, YearPurchaseSelected, YearCreateSelected, SelectedTypeVehicle);
            if (!HasErrors)
            {
                int MileageInt = ConverFromInt(Mileage);
                _listRepairType.RemoveAll(c => c.IsRemoveMaintenance == true);


                var vehicle = new Vehicle
                    (
                    NameVehicle, YearCreateSelected,
                    YearPurchaseSelected, VinCode,
                    StateNumber, TransmissionType,
                    SelectedTypeVehicle, MileageInt,
                    _listRepairType
                    );
                vehicle.PhotoVehicle = _photoVehicle ?? String.Empty;

                if (_isUpdateVehicle)
                {
                    vehicle.Id = _vehicle.Id;
                    await EditVehicle(vehicle);
                }
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
            else await _dialogService.ShowToastAsync("Ошибка. Проверте все поля");
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
        /// Работа с кнопкой полностью авто обслужено
        /// </summary>
        [RelayCommand]
        public void ChangeIsServiced()
        {
            foreach (var list in _listRepairType)
            {
                list.IsServiced = !list.IsServiced;
            }
        }

        /// <summary>
        /// Добавление фото для авто
        /// </summary>
        [RelayCommand]
        public async void PickPhoto()
        {
            var photoLocation = await _photoPicker.PickPhotoAsync();
            if (photoLocation == string.Empty) { }
            else
            {
                PathPhoto = photoLocation;
                _photoVehicle = photoLocation;
            }
        }


        #region Методы CommunityToolKit

        partial void OnTransmissionTypeChanged(string value)
        {
            foreach (var item in _listRepairType)
            {
                if (item.TransmissionType == "Автоматическая" || item.TransmissionType == "Механическая")
                {
                    item.IsRemoveMaintenance = item.TransmissionType != value;
                }
            }
        }
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
        partial void OnSelectedTypeVehicleChanged(string value)
        {
            _vehicleValidation.ValidationTypeVehicle(value);
        }
        #endregion


        /// <summary>
        /// Метод которые вызывает event 
        /// </summary>
        /// <param name="e"></param>
        private void OnErrorsChangedUI(DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(e.PropertyName);
            CheckErrorsChanged();
        }

        /// <summary>
        /// Переключение bool для текста ошибок
        /// </summary>
        private void CheckErrorsChanged()
        {
            if(MileageError.Any()) IsMileageError = true;
            else IsMileageError = false;
            if(YearPurchaseError.Any()) IsYearPurchaseError = true;
            else IsYearPurchaseError = false;
            if(TypeVehicleError.Any()) IsTypeVehicleError = true;
            else IsTypeVehicleError = false;
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

        /// <summary>
        /// Создание RepairType
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<RepairGroup> CreateRepairTypeGroups()
        {
            var groups = new ObservableCollection<RepairGroup>
    {
        new RepairGroup("Регулярное ТО (Расходники)", new List<RepairType>
        {
            // Самое частое. Масло - раз в год или 10к, Фильтры - вместе с ним
            new("Масло в двигателе и Масляный фильтр", "Регулярное ТО", 10000, 12),
            new("Воздушный фильтр двигателя", "Регулярное ТО", 20000, 24),
            new("Салонный фильтр", "Регулярное ТО", 15000, 12),
            new("Топливный фильтр", "Регулярное ТО", 40000, 48)
        }),

        new RepairGroup("Тормозная система", new List<RepairType>
        {
            // Тормозная жидкость стареет именно от ВРЕМЕНИ (впитывает влагу)
            new("Тормозная жидкость", "Тормозная система", 40000, 24),
            new("Передние тормозные колодки", "Тормозная система", 30000),
            new("Задние тормозные колодки", "Тормозная система", 50000),
            new("Передние тормозные диски + колодки", "Тормозная система", 70000),
            new("Задние тормозные диски + колодки", "Тормозная система", 90000),
            new("Задние барабаны + колодки + тормозной цилиндр", "Тормозная система", 90000),
            new("Обслуживание суппортов (смазка)", "Тормозная система", 30000, 24),
            new("Обслуживание передних тормозов", "Тормозная система", 0),
            new("Обслуживание задних тормозов", "Тормозная система", 0)
        }),

        new RepairGroup("Двигатель и Зажигание", new List<RepairType>
        {
            // Обобщаем ремни и цепи
            new("Свечи зажигания / накаливания", "Двигатель и Зажигание", 40000, 48),
            new("Привод ГРМ (Ремень / Цепь)", "Двигатель и Зажигание", 90000, 60),
            new("Ремни навесного оборудования", "Двигатель и Зажигание", 60000, 60),
            new("Регулировка клапанов", "Двигатель и Зажигание", 80000)
        }),

        new RepairGroup("Охлаждение и Климат", new List<RepairType>
        {
            // Разделили помпу и антифриз
            new("Охлаждающая жидкость", "Охлаждение и Климат", 60000, 36),
            new("Водяная помпа (Насос)", "Охлаждение и Климат", 90000, 60),
            new("Промывка радиаторов", "Охлаждение и Климат", 60000, 24),
            new("Обслуживание кондиционера (фреон)", "Охлаждение и Климат", 40000, 24)
        }),

        new RepairGroup("Трансмиссия (Коробка и Привод)", new List<RepairType>
        {
            // Универсальные названия
            new("Масло в коробке передач", "Трансмиссия", 60000, 48),
            new("Фильтр коробки передач", "Трансмиссия", 60000, 48,"Автоматическая"),
            new("Сброс адаптации", "Трансмиссия", 60000, 48,"Автоматическая"),
            new("Масло в редукторе / мосту", "Трансмиссия", 60000, 48),
            new("Масло в раздаточной коробке", "Трансмиссия", 60000, 48),
            new("Сцепление", "Трансмиссия", 100000)
        }),

        new RepairGroup("Подвеска и Рулевое", new List<RepairType>
        {
            new("Жидкость ГУР", "Подвеска и Рулевое", 50000, 36),
            new("Передние амортизаторы", "Подвеска и Рулевое", 80000),
            new("Задние амортизаторы", "Подвеска и Рулевое", 90000),
            new("Стойки и втулки стабилизатора", "Подвеска и Рулевое", 40000),
            new("Сайлентблоки (комплект)", "Подвеска и Рулевое", 80000),
            new("Шаровые опоры", "Подвеска и Рулевое", 70000),
            new("Рулевые наконечники и тяги", "Подвеска и Рулевое", 70000)
        }),

        new RepairGroup("Шины и Колеса", new List<RepairType>
        {
            new("Сход-развал", "Шины и Колеса", 20000, 12),
            new("Балансировка колес", "Шины и Колеса", 10000, 6),
            new("Сезонная смена шин", "Шины и Колеса", 0, 6)
        }),

        new RepairGroup("Кузов и Оптика", new List<RepairType>
        {
            new("Щетки стеклоочистителя", "Кузов и Оптика", 15000, 12),
            new("Обработка кузова (Антикор)", "Кузов и Оптика", 0, 36),
            new("Замена ламп", "Кузов и Оптика")
        })
    };

            return groups;
        }
    }
}
