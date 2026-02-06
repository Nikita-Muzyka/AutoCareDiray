using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Models;
using AutoCareDiray.Service;
using AutoCareDiray.View;
using AutoCareDiray.View.Maintenanse;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoCareDiray.ViewModels.Cars
{
    [QueryProperty(nameof(CarSelected),"SelCar")]
    public partial class CarCardViewModal : BaseViewModel
    {
        public Car CarSelected;
        public CarCardViewModal(IApiService apiService, IDialogService dialogService) : base(apiService, dialogService) 
        {
            
        }

        [RelayCommand]
        public async void CreateMaintenanse()
        {
            var Car = new Dictionary<string, object>()
            {
                ["Car"] = CarSelected
            };
            await Shell.Current.GoToAsync(nameof(CreateMaintenanse), Car);
        }
        [RelayCommand]
        public async void ListMaintenanse()
        {
            
            await Shell.Current.GoToAsync(nameof(ListMaintenanseView));
        }
        [RelayCommand]
        public async void GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
