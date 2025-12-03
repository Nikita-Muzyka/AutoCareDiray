using AutoCareDiray.Service;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels
{
    public partial class CreateMaintenanseViewModal
    {
        private readonly IApiService _apiService;
        public CreateMaintenanseViewModal(IApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        public void CreateMaintenanse()
        {

        }
    }
}
