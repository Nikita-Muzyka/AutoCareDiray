using AutoCareDiray.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels.Cars
{
    public partial class ListCarsViewModal
    {
        private readonly IApiService _apiService;
        public ListCarsViewModal(IApiService apiService)
        {
            _apiService = apiService;
        }
    }
}
