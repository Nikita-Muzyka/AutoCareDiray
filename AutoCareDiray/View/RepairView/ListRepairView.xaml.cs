
using AutoCareDiray.Extensions;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.ViewModels.RepairViewModel;

namespace AutoCareDiray.View.RepairView;

public partial class ListRepairView : ContentPage
{
    ListRepairViewModel _viewModel;
	public ListRepairView(ListRepairViewModel vm)
	{
		InitializeComponent();
        _viewModel = vm;
		BindingContext = _viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		if(BindingContext is ListRepairViewModel vm)
		{
			vm.StartLoadingCommand.Execute(null);
		}
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is ListRepairViewModel vm)
        {
            vm.CancelTokenCommand.Execute(null);
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Grid cardGrid)
        {
            await cardGrid.CardBounceAsync();

            var selectedRepair = e.Parameter as Repair;

            _viewModel.GoRepairCardCommand.Execute(selectedRepair);
        }
    }
}