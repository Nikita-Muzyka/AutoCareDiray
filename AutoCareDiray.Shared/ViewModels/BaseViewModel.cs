using AutoCareDiray.Shared.Interface;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoCareDiray.Shared.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        //protected readonly IApiService _apiService;
        protected readonly IDataService _dataService;
        protected readonly IDialogService _dialogService;
        protected readonly INavigationService _navigationService;
        public BaseViewModel(IDialogService dialogService, IDataService dataService,INavigationService navigation)
        {
            _dialogService = dialogService;
            _dataService = dataService;
            _navigationService = navigation;
        }
    }
}
