using AutoCareDiray.Models;
using AutoCareDiray.Service;
using AutoCareDiray.Service.Data;
using AutoCareDiray.Service.Navigation;
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
        protected readonly IDataService _dataService;
        protected readonly IDialogService _dialogService;
        protected readonly INavigationService _navigationService;
        public BaseViewModel(IApiService apiService, IDialogService dialogService, IDataService dataService,INavigationService navigation)
        {
            _apiService = apiService;
            _dialogService = dialogService;
            _dataService = dataService;
            _navigationService = navigation;
        }
    }
}
