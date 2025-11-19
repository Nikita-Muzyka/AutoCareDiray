using AutoCareDiray.Models;
using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;
using AutoCareDiray.ViewModels.Cars;
namespace AutoCareDiray.View;

public partial class CarCardPage : ContentPage
{
	public CarCardPage(Car car,IApiService apiService)
	{
		InitializeComponent();
		BindingContext = new CarCardViewModal(car, apiService);
	}
}