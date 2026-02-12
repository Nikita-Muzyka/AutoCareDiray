using AutoCareDiray.Service;

namespace AutoCareDiray.ViewModels.VehicleViewModel
{
    //[QueryProperty(nameof(CarSelected),"SelCar")]
    public partial class CardVehicleViewModel : BaseViewModel
    {
        //public Car CarSelected;
        public CardVehicleViewModel(IApiService apiService, IDialogService dialogService) : base(apiService, dialogService)
        {

        }

        //[RelayCommand]
        //public async void CreateMaintenanse()
        //{
        //    var Car = new Dictionary<string, object>()
        //    {
        //        ["Car"] = CarSelected
        //    };
        //    await Shell.Current.GoToAsync(nameof(CreateMaintenanse), Car);
        //}
        //[RelayCommand]
        //public async void ListMaintenanse()
        //{

        //    await Shell.Current.GoToAsync(nameof(ListMaintenanseView));
        //}
        //[RelayCommand]
        //public async void GoBack()
        //{
        //    await Shell.Current.GoToAsync("..");
        //}
    }
}
