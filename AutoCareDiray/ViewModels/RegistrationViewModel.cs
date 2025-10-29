using AutoCareDiray.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels
{
    class RegistrationViewModel
    {
        private readonly IApiService _apiService;
        public RegistrationViewModel(IApiService api) 
        {
            _apiService = api;
        }
    }
}
