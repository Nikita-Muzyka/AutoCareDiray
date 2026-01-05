using AutoCareDiray.Models;
using AutoCareDiray.Models.Validation;
using AutoCareDiray.Service;
using AutoCareDiray.View;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels
{
    public partial class CreateCarsViewModal : BaseViewModel
    {
        public CarValidation _carValidation;
        private CancellationTokenSource _cts;

        //[ObservableProperty]
        //public string[] brands = CarBrands.Brands;
        [ObservableProperty]
        public string brandSelected;
        [ObservableProperty]
        public string modelSelected;
        [ObservableProperty]
        public string yearSelected;
        [ObservableProperty]
        public string vinCode;
        [ObservableProperty]
        public string mileage;
        [ObservableProperty]
        public string transmissionBoxSelected;
        [ObservableProperty]
        public string engineTypeSelected;
        [ObservableProperty]
        public string yearPuchaseSelected;

        [ObservableProperty]
        public string errorsAll;

     
        public CreateCarsViewModal(IApiService apiService,IDialogService dialogService) : base(apiService, dialogService)
        {
            _carValidation = new();
            _carValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI(e);
            _cts = new CancellationTokenSource();
        }

        // Получение ошибок
        public bool HasErrors => _carValidation.HasErrors;
        public string VinCodeError => _carValidation.GetErrors("VinCode") as string;
        public string MileageError => _carValidation.GetErrors("Mileage") as string;

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
        public async void CreateCar()
        {
            _carValidation.ValidationAll(VinCode, Mileage);
            if (!HasErrors)
            {
                try
                {
                    var car = CreateClassCar();
                    var response = await _apiService.CreateCarApiAsync(car,_cts.Token);

                    ErrorsAll = response.Message;
                }
                catch (Exception ex)
                {
                    errorsAll = ex.Message;
                }
            }
        }

        [RelayCommand]
        public async void BackGo()
        {
            await Shell.Current.GoToAsync("..");
        }

        //методы Community Tool
        partial void OnVinCodeChanged(string value)
        {
            _carValidation.ValidationVinCode(value);
        }
        partial void OnMileageChanged(string value)
        {
            _carValidation.ValidationMileage(value);
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

        /// <summary>
        /// Создание авто
        /// </summary>
        /// <returns></returns>
        Car CreateClassCar()
        {
            int YearInt = ConverFromInt(YearSelected);
            int MileageInt = ConverFromInt(Mileage); 
            int YearPurchaseint = ConverFromInt(YearPuchaseSelected);

                var car = new Car
                {
                    User_id = Preferences.Get("User_id", 0),

                    Brand = BrandSelected,
                    Model = ModelSelected,
                    Year = YearInt,
                    Vin = vinCode,

                    Current_mileage = MileageInt,
                    Year_purchase = YearPurchaseint,

                    Transmission_box = TransmissionBoxSelected,
                    Engine_type = EngineTypeSelected,
                };

                return car;
        }
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
}
