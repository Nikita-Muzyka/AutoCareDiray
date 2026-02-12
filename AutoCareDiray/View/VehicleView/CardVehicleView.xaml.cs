using AutoCareDiray.ViewModels.VehicleViewModel;

namespace AutoCareDiray.View.VehicleView;

public partial class CardVehicleView : ContentPage
{
	public CardVehicleView(CardVehicleViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}