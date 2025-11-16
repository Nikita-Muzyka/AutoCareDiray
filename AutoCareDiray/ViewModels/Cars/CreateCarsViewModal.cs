using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Service;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.View;
using AutoCareDiray.Models;
using AutoCareDiray.Models.Car;

namespace AutoCareDiray.ViewModels
{
    public partial class CreateCarsViewModal : ObservableObject
    {
        private readonly IApiService _apiService;
        public CarValidation _carValidation;
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


        public CreateCarsViewModal(IApiService apiService) 
        {
            _apiService = apiService;
            _carValidation = new();
            _carValidation.ErrorsChanged += (s, e) => OnErrorsChangedUI();
        }

        public bool HasErrors => _carValidation.HasErrors;
        public string VnCodeError => _carValidation.GetErrors("VinCode") as string;
        public string MileageError => _carValidation.GetErrors("Mileage") as string;


        public async void CreateCar()
        {
            _carValidation.ValidationAll(VnCode, Mileage);
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
    }
}
