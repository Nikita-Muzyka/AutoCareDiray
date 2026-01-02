using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;
using AutoCareDiray.ViewModels.Cars;

namespace AutoCareDiray.View;

public partial class ListCars : ContentPage
{
	public ListCars(ListCarsViewModal list)
	{
		InitializeComponent();
		BindingContext = list;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ListCarsViewModal list)
        {
            list.LoadCarsCommand.Execute(null);
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if(BindingContext is ListCarsViewModal list)
        {
            list.CancelToken();
        }
    }
    
}