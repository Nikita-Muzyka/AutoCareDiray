using AutoCareDiray.Shared.ViewModels.VehicleViewModel;

namespace AutoCareDiray.View.VehicleView;

public partial class ListVehicleView : ContentPage
{
	public ListVehicleView(ListVehicleViewModel list)
	{
		InitializeComponent();
		BindingContext = list;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ListVehicleViewModel list)
        {
           list.LoadVehiclesCommand.Execute(null);
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if(BindingContext is ListVehicleViewModel list)
        {
            list.CancelTokenCommand?.Execute(null);
        }
    }
    
}