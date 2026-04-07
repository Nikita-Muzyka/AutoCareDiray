using AutoCareDiray.Extensions;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.ViewModels.VehicleViewModel;
using System.Threading.Tasks;

namespace AutoCareDiray.View.VehicleView;

public partial class ListVehicleView : ContentPage
{
    ListVehicleViewModel _viewModel;
	public ListVehicleView(ListVehicleViewModel list)
	{
		InitializeComponent();
        _viewModel = list;
		BindingContext = _viewModel;
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


    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Grid cardGrid)
        {
            await cardGrid.CardBounceAsync();

            var selectedVehicle = e.Parameter as Vehicle;

            _viewModel.GoCarCardCommand.Execute(selectedVehicle);
        }
    }
}