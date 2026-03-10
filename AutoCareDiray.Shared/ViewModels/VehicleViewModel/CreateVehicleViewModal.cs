using AutoCareDiray.Shared.Models.Validation;
using AutoCareDiray.Shared.Models.VehicleModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AutoCareDiray.Shared.Service.Data;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Interface;


namespace AutoCareDiray.Shared.ViewModels.VehicleViewModel
{
    public partial class CreateVehicleViewModel : BaseViewModel
    {
        private VehicleValidation _vehicleValidation;
        private CancellationTokenSource _cts;

        private Vehicle _vehicle;
        private bool _isInitilized = false;
        private bool _isEditVehicle = false;

        [ObservableProperty]
        private string buttonName = "Создать";
        [ObservableProperty]
        private string nameVehicle = String.Empty;
        [ObservableProperty]
        private DateTime yearCreateSelected = DateTime.Today;
        [ObservableProperty]
        private string mileage = String.Empty;
        [ObservableProperty]
        private DateTime yearPurchaseSelected = DateTime.Today;
        [ObservableProperty]
        private string selectedTypeVehicle = String.Empty;
        [ObservableProperty]
        private DateTime dateNow = DateTime.Today;

        [ObservableProperty]
        private string statusMessage;

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

        [RelayCommand]
        public async Task Initilize(int VehicleId)
        {
            _isEditVehicle = true;
            if (_isInitilized == false)
            {
                if (VehicleId >= 0)
                {
                    _vehicle = await _dataService.GetVehicleAsync(VehicleId, _cts.Token);
                    if (_vehicle.Id >= 0)
                    {
                        _isInitilized = true;
                        ButtonName = "Изменить";

                        NameVehicle = _vehicle.NameVehicle;
                        YearCreateSelected = _vehicle.YearCreate;
                        YearPurchaseSelected = _vehicle.YearPurchase;
                        Mileage = _vehicle.Mileage.ToString();
                        SelectedTypeVehicle = _vehicle.VehicleType;
                    }
                }
            }
        }
        [RelayCommand]
        public async Task CreateVehicle()
        {
            _vehicleValidation.ValidationAll(Mileage, YearPurchaseSelected, YearCreateSelected);
            if (!HasErrors)
            {
                int MileageInt = ConverFromInt(Mileage);

                var vehicle = new Vehicle
                {
                    NameVehicle = NameVehicle,
                    Mileage = MileageInt,
                    YearCreate = YearCreateSelected,
                    YearPurchase = YearPurchaseSelected,
                    VehicleType = SelectedTypeVehicle,
                };
                if (_isEditVehicle)
                {
                    vehicle.Id = _vehicle.Id;
                    await EditVehicle(vehicle);
                }
                else
                {
                    CreateRepairTypes(vehicle);
                   var result =  await _dataService.CreateVehicleAsync(vehicle, _cts.Token);
                    if (result) await _navigationService.GoToBack();
                    else await _dialogService.ShowToastAsync("Не удалось создать авто");
                }
            }
        }

        public async Task EditVehicle(Vehicle vehicle)
        {
            var result = await _dataService.UpdateVehicleAsync(vehicle, _cts.Token);
            if(result)
            {
                await _dialogService.ShowToastAsync("Данные обновлены");
                await _navigationService.GoToBack();
            }
        }

        //методы Community Tool
      
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

        private void CreateRepairTypes(Vehicle vehicle)
        {
            vehicle.ReepairTypes = new List<RepairType>()
            {
                // ТО и сезонные
                new("ТО", 15000),
                new("Переобувка", 0, new DateTime(1, 4, 1)),  // Каждый апрель

                // Масло и фильтры
                new("Масло и масляный фильтр", 8000, new DateTime(1, 1, 1).AddYears(1)),
                new("Воздушный фильтр", 20000),
                new("Салонный фильтр", 15000),
                new("Топливный фильтр", 40000),

                // Тормозная система
                new("Тормозные колодки передние", 30000),
                new("Тормозные колодки задние", 50000),
                new("Тормозные диски передние", 60000),
                new("Тормозные диски задние", 80000),
                new("Тормозная жидкость", 40000),

                // Двигатель
                new("Свечи зажигания", 30000),
                new("Ремень ГРМ", 90000),
                new("Цепь ГРМ", 150000),
                new("Ремень генератора", 60000),
                new("Катушки зажигания", 100000),

                // Охлаждение
                new("Антифриз", 60000),
                new("Термостат", 100000),
                new("Помпа", 90000),
                new("Радиатор", 150000),

                // Трансмиссия
                new("Масло в АКПП/CVT", 60000),
                new("Масло в МКПП", 80000),
                new("Сцепление", 100000),

                // Подвеска и рулевое
                new("Стойки амортизаторов передние", 80000),
                new("Стойки амортизаторов задние", 100000),
                new("Сайлентблоки", 80000),
                new("Шаровые опоры", 80000),
                new("Стойки стабилизатора", 50000),
                new("Рулевые наконечники", 60000),
                new("Рулевые тяги", 80000),
                new("Ступичный подшипник", 100000),
                new("ШРУС (граната)", 100000),
                new("Пыльник ШРУСа", 50000),

                // Электрика
                new("Аккумулятор (АКБ)", 70000),
                new("Генератор", 150000),
                new("Стартер", 150000),

                // Шины
                new("Развал-схождение", 15000),
                new("Балансировка колёс", 15000),

                // Кузов
                new("Дворники (щётки)", 20000),
                new("Лампы (фары/габариты)", 50000),

                // Выхлоп
                new("Катализатор", 150000),
                new("Лямбда-зонд", 100000),
                new("Система отвода газов", 120000),

                // Прочее
                new("ГУР жидкость", 60000),
                new("Кондиционер (заправка)", 40000),

            };
        }
    }
}
