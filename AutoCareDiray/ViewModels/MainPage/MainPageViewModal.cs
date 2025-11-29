using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Service;
using AutoCareDiray.View;

namespace AutoCareDiray.ViewModels
{
    public partial class MainPageViewModal
    {
        private readonly IApiService _apiService;
        private readonly IDialogService _dialogService;
        public MainPageViewModal(IApiService apiService,IDialogService dialogService)
        {
            _apiService = apiService;
            _dialogService = dialogService;
        }
    }
}
