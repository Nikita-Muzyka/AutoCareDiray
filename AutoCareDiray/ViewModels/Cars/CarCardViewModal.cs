using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Models;
using AutoCareDiray.Service;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoCareDiray.ViewModels.Cars
{
    public partial class CarCardViewModal : ObservableObject
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        Car car;
        public CarCardViewModal(Car car, IApiService apiService)
        {
            this.car = car;
            _apiService = apiService;
        }
    }
}
