using AutoCareDiray.ViewModels.VehicleViewModel;

namespace AutoCareDiray.View.VehicleView;

public partial class CreateVehicleView : ContentPage
{
	public CreateVehicleView(CreateVehicleViewModel createCar)
	{
		InitializeComponent();
		BindingContext = createCar;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if(BindingContext is CreateVehicleViewModel createCar)
        {
            createCar.CancelToken();
        }
    }
}