using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;
using AutoCareDiray.ViewModels.Cars;

namespace AutoCareDiray.View;

public partial class ListCars : ContentPage
{
	public ListCars(IApiService apiService)
	{
		InitializeComponent();
		BindingContext = new ListCarsViewModal(apiService);
	}

    
}