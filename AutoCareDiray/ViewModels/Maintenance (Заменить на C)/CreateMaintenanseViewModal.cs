using AutoCareDiray.Models;
using AutoCareDiray.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.ViewModels
{
    //[QueryProperty(nameof(SelectedCar),"Car")]
    public partial class CreateMaintenanseViewModal
    {
        //[ObservableProperty]
        //public Car selectedCar;

        //[ObservableProperty]
        //public string serviceDate;

        //[ObservableProperty]
        //public string mileage;

        //[ObservableProperty]
        //public string serviceType;

        //[ObservableProperty]
        //public string description;

        //[ObservableProperty]
        //public string cost;

        //[ObservableProperty]
        //public string serviceCentre;

        //[ObservableProperty]
        //public string text;

        //public CreateMaintenanseViewModal(IApiService apiService, IDialogService dialogService) :base(apiService, dialogService)
        //{

        //}

        //[RelayCommand]
        //public async void CreateMaintenanse()
        //{
        //    var date = DateTime.Parse(ServiceDate);
        //    var mileage = int.Parse(Mileage);
        //    var cost = int.Parse(Cost);

        //    var maintenanse = new MaintenanseModel(SelectedCar.Car_id, date, mileage, ServiceType, Description, cost, ServiceCentre);
        //    Text = "Запись создана";
        //}
        //[RelayCommand]
        //public async void GoBack()
        //{
        //    await Shell.Current.GoToAsync("..");
        //}
    }
}
