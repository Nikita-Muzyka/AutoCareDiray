using AutoCareDiray.Models;
using AutoCareDiray.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        protected readonly IApiService _apiService;
        protected readonly IDialogService _dialogService;
        public BaseViewModel(IApiService apiService,IDialogService dialogService)
        {
            _apiService = apiService;
            _dialogService = dialogService;
        }
    }
}
