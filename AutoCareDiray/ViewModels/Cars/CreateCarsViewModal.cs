using AutoCareDiray.Models;
using AutoCareDiray.Service;
using AutoCareDiray.View;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels
{
    public partial class CreateCarsViewModal : ObservableObject
    {
        private readonly IApiService _apiService;
        public CarValidation _carValidation;
        public CarResponse _carResponse;
        //[ObservableProperty]
        //public string[] brands = CarBrands.Brands;
        [ObservableProperty]
        public string brandSelected;
        [ObservableProperty]
        public string modelSelected;
        [ObservableProperty]
        public string yearSelected;
        [ObservableProperty]
        public string vnCode;
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


        public CreateCarsViewModal(IApiService apiService) 
        {
            _apiService = apiService;
            _carValidation = new();
            _carValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI();
        }

        public bool HasErrors => _carValidation.HasErrors;
        public string VnCodeError => _carValidation.GetErrors("VinCode") as string;
        public string MileageError => _carValidation.GetErrors("Mileage") as string;

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
            _carValidation.ValidationAll(VnCode, Mileage);
            if (!HasErrors)
            {
                try
                {
                    var car = CreateClassCar();
                    var response = await _apiService.CreateCarApiAsync(car);

                    ErrorsAll = response.Message;
                }
                catch (Exception ex)
                {
                    errorsAll = ex.Message;
                }
            }
        }

        partial void OnVnCodeChanged(string value)
        {
            _carValidation.ValidationVinCode(value);
        }
        partial void OnMileageChanged(string value)
        {
            _carValidation.ValidationMileage(value);
        }
        void OnErrorsChangedUI()
        {
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(VnCodeError));
            OnPropertyChanged(nameof(MileageError));
        }

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
                    Vin = VnCode,

                    Current_mileage = MileageInt,
                    Year_purchase = YearPurchaseint,

                    Transmission_box = TransmissionBoxSelected,
                    Engine_type = EngineTypeSelected,
                };

                return car;
        }
    }
}
