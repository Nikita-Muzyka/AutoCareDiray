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
    public partial class CreateMaintenanseViewModal : ObservableObject
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        public string serviceDate;

        [ObservableProperty]
        public string mileage;

        [ObservableProperty]
        public string serviceType;

        [ObservableProperty]
        public string description;

        [ObservableProperty]
        public string cost;

        [ObservableProperty]
        public string serviceCentre;

        [ObservableProperty]
        public string text;

        int Car_Id { get; set; }
        public CreateMaintenanseViewModal(IApiService apiService,int car_id)
        {
            _apiService = apiService;
            Car_Id = car_id;
        }

        [RelayCommand]
        public async void CreateMaintenanse()
        {
            var date = DateTime.Parse(ServiceDate);
            var mileage = int.Parse(Mileage);
            var cost = int.Parse(Cost);

            var maintenanse = new Maintenanse(Car_Id, date, mileage, ServiceType, Description, cost, ServiceCentre);
            Text = "Запись создана";
        }
        [RelayCommand]
        public async void GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
