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


namespace AutoCareDiray.Shared.ViewModels.VehicleViewModel
{
    public partial class CreateVehicleViewModel : BaseViewModel
    {
        private VehicleValidation _vehicleValidation;
        private CancellationTokenSource _cts;
        private Vehicle _vehicle;
        private List<RepairType> _listRepairType;
        private ObservableCollection<RepairGroup> _grouped;
        //Инициализирована ли страница
        private bool _isInitilized = false;
        //переключатель с создания авто на обновление данных авто
        private bool _isUpdateVehicle = false;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))] // Теперь он должен его видеть
        private bool isBusy;

        // Это свойство ты прописываешь сам, и оно должно быть PUBLIC
        public bool IsNotBusy => !IsBusy;

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
        private string statusMessage;

        [ObservableProperty]
        private ObservableCollection<RepairGroup> repairGrouped;

        public ObservableCollection<string> TypeVehicle { get; } = new() { "Автомобиль", "Мотоцикл", "Грузовое ТС", "Другое" };



        public CreateVehicleViewModel(IDialogService dialogService,IDataService dataService,INavigationService navigation,
            VehicleValidation vehicleValidation) : base(dialogService,dataService,navigation)
        {
            _vehicleValidation = vehicleValidation;
            _vehicleValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
            _cts = new CancellationTokenSource();
            SelectedTypeVehicle = TypeVehicle.FirstOrDefault(String.Empty);
        }

        // Получение ошибок
        public bool HasErrors => _vehicleValidation.HasErrors;
        public string MileageError => _vehicleValidation.GetErrors(nameof(MileageError)) as string ?? String.Empty;
        public string YearPurchaseError => _vehicleValidation.GetErrors(nameof(YearPurchaseError)) as string ?? String.Empty;

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

                _isUpdateVehicle = true;
                _isInitilized = true;
                ButtonName = "Изменить";

                NameVehicle = _vehicle.NameVehicle;
                YearCreateSelected = _vehicle.YearCreate;
                YearPurchaseSelected = _vehicle.YearPurchase;
                Mileage = _vehicle.Mileage.ToString();
                SelectedTypeVehicle = _vehicle.VehicleType;
                VinCode = _vehicle.VinCode;
            }
        }

        /// <summary>
        /// Инициализация при создании авто
        /// </summary>
        [RelayCommand]
        public async Task InitilizeForCreateVeicle()
        {
            if (_isInitilized) return;
            IsBusy = true;
            try
            {

                await Task.Delay(100);
                var groups = await Task.Run(() => CreateRepairTypeGroups());

                RepairGrouped = new ObservableCollection<RepairGroup>(groups);

                _listRepairType = RepairGrouped.SelectMany(c => c).ToList();
                _isInitilized = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки: {ex.Message}");
            }
            finally
            {
                await Task.Delay(1000);
                IsBusy = false;
            }
        }


        [RelayCommand]
        public async Task CreateVehicle()
        {
            _vehicleValidation.ValidationAll(Mileage, YearPurchaseSelected, YearCreateSelected);
            if (!HasErrors)
            {
                int MileageInt = ConverFromInt(Mileage);
                _listRepairType.RemoveAll(c => c.IsRemoveMaintenance == true);
               

                    var vehicle = new Vehicle(NameVehicle, YearCreateSelected,
                        YearPurchaseSelected, VinCode,
                        StateNumber, TransmissionType,
                        SelectedTypeVehicle, MileageInt,
                        _listRepairType);

                if (_isUpdateVehicle)
                {
                    vehicle.Id = _vehicle.Id;
                    await EditVehicle(vehicle);
                }
                else
                {
                   var result =  await _dataService.CreateVehicleAsync(vehicle, _cts.Token);

                    if (result.Success)
                    {
                        await _dialogService.ShowToastAsync("ТС создано");
                        await _navigationService.GoToBack();
                    }
                    else StatusMessage = result.ErrorMessage;
                }
            }
        }

        [RelayCommand]
        public void ChangeIsServiced()
        {
            foreach (var list in _listRepairType)
            {
                list.IsServiced = !list.IsServiced;
            }
        }

        public async Task EditVehicle(Vehicle vehicle)
        {
            var result = await _dataService.UpdateVehicleAsync(vehicle, _cts.Token);
            if(result.Success)
            {
                await _dialogService.ShowToastAsync("Данные обновлены");
                await _navigationService.GoToBack();
            }
            else StatusMessage = result.ErrorMessage;
        }

        //методы Community Tool

        partial void OnTransmissionTypeChanged(string value)
        {
            if (value == "Автоматическая")
            {
                foreach (var list in _listRepairType.Where(c => c.Category == "Трансмиссия и Жидкости").ToList())
                {
                    if(list.TransmissionType == "Автоматическая")
                    {
                        list.IsRemoveMaintenance = false;
                    }
                    if (list.TransmissionType == "Механическая")
                    {
                        list.IsRemoveMaintenance = true;
                    }
                }
            }
            else
            {
                foreach (var list in _listRepairType.Where(c => c.Category == "Трансмиссия и Жидкости").ToList())
                {
                    if (list.TransmissionType == "Механическая")
                    {
                        list.IsRemoveMaintenance = false;
                    }
                    if (list.TransmissionType == "Автоматическая")
                    {
                        list.IsRemoveMaintenance = true;
                    }
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

        /// <summary>
        /// Метод которые вызывает event 
        /// </summary>
        /// <param name="e"></param>
        void OnErrorsChangedUI(DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(e.PropertyName);
        }

        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }

        private ObservableCollection<RepairGroup> CreateRepairTypeGroups()
        {
            var groups = new ObservableCollection<RepairGroup>
            {
                new RepairGroup("Регламентное ТО", new List<RepairType>
                {
                    new("ТО (Общее)","ТО", 15000),
                    new("Масло и масляный фильтр","ТО", 8000),
                    new("Воздушный фильтр","ТО", 20000),
                    new("Салонный фильтр","ТО", 15000),
                    new("Топливный фильтр","ТО", 40000)
                }),

                new RepairGroup("Тормозная система", new List<RepairType>
                {
                    new("Тормозные колодки передние","Тормозная система", 30000),
                    new("Тормозные колодки задние","Тормозная система", 50000),
                    new("Тормозные диски передние","Тормозная система", 60000),
                    new("Тормозные диски задние","Тормозная система", 80000),
                    new("Тормозная жидкость","Тормозная система", 40000),
                    new("Обслуживвание задних тормозов","Тормозная система"),
                    new("Обслуживвание передних тормозов","Тормозная система")
                }),

                new RepairGroup("Двигатель и ГРМ", new List<RepairType>
                {
                    new("Свечи зажигания","Двигатель и ГРМ", 30000),
                    new("Ремень ГРМ","Двигатель и ГРМ", 90000),
                    new("Ремень ГРМ + Помпа","Двигатель и ГРМ", 90000),
                    new("Цепь ГРМ","Двигатель и ГРМ", 150000),
                    new("Ремень генератора","Двигатель и ГРМ", 60000),
                    new("Катушки зажигания","Двигатель и ГРМ", 100000)
                }),

                new RepairGroup("Подвеска и Рулевое", new List<RepairType>
                {
                    new("Стойки амортизаторов передние","Подвеска и Рулевое", 80000),
                    new("Стойки амортизаторов задние","Подвеска и Рулевое", 100000),
                    new("Сайлентблоки","Подвеска и Рулевое", 80000),
                    new("Шаровые опоры","Подвеска и Рулевое", 80000),
                    new("Стойки стабилизатора","Подвеска и Рулевое", 50000),
                    new("Рулевые наконечники","Подвеска и Рулевое", 60000),
                    new("Ступичный подшипник","Подвеска и Рулевое", 100000),
                    new("Обслуживание передней подвески","Подвеска и Рулевое"),
                    new("Обслуживание задний подвески","Подвеска и Рулевое"),
                }),

                new RepairGroup("Трансмиссия и Жидкости", new List<RepairType>
                {
                    new("Масло в АКПП/CVT","Трансмиссия и Жидкости", 60000,"Автоматическая"),
                    new("Масло в МКПП","Трансмиссия и Жидкости", 80000),
                    new("Ремонт коробки","Трансмиссия и Жидкости", 60000),
                    new("Сцепление","Трансмиссия и Жидкости", 100000,"Механическая"),
                    new("Замена ремня АКПП","Трансмиссия и Жидкости", 100000,"Автоматическая"),
                    new("ГУР жидкость","Трансмиссия и Жидкости", 60000, new DateTime(2)),
                    new("Антифриз","Трансмиссия и Жидкости", 60000, new DateTime(3)),
                    new("Тосол","Трансмиссия и Жидкости", 60000, new DateTime(2)),
                    new("Тормозная жидкость","Трансмиссия и Жидкости", 60000, new DateTime(4))
                }),

                new RepairGroup("Электрика и Охлаждение", new List<RepairType>
                {
                    new("Аккумулятор (АКБ)","Электрика и Охлаждение", 70000),
                    new("Генератор","Электрика и Охлаждение", 150000),
                    new("Стартер","Электрика и Охлаждение", 150000),
                    new("Помпа","Электрика и Охлаждение", 90000),
                    new("Термостат","Электрика и Охлаждение", 100000),
                    new("Замена датчика","Электрика и Охлаждение", 0)
                }),

                new RepairGroup("Шины и Колеса", new List<RepairType>
                {
                    new("Развал-схождение","Шины и Колеса", 15000),
                    new("Балансировка колёс","Шины и Колеса", 15000),
                    new("Переобувка","Шины и Колеса", 0) // Здесь можно добавить логику по дате
                }),

                new RepairGroup("Выхлопная система", new List<RepairType>
                {
                    new("Катализатор","Выхлопная система"),
                    new("Выпускной коллектор","Выхлопная система"),
                    new("Датчики выпускной системы","Выхлопная система") // Здесь можно добавить логику по дате
                }),

                new RepairGroup("Прочее", new List<RepairType>
                {
                    new("Дворники (щётки)","Прочее"),
                    new("Лампы (фары/габариты)","Прочее"),
                    new("Кондиционер (заправка)", "Прочее", 40000)
                })
            };
            return groups;
        }
    }
}
