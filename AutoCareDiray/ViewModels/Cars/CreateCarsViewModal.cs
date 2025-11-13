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

namespace AutoCareDiray.ViewModels
{
    public partial class CreateCarsViewModal : ObservableObject
    {
        private readonly IApiService _apiService;

        //[ObservableProperty]
        //public string[] brands = CarBrands.Brands;
        //[ObservableProperty]
        //public string brandSelected;
        public CreateCarsViewModal(IApiService apiService) 
        {
            _apiService = apiService;
        }
    }
}
