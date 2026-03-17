using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.ViewModels.VehicleViewModel;

namespace AutoCareDiray.View.VehicleView;

public partial class CreateVehicleView : ContentPage,IQueryAttributable
{
    private readonly CreateVehicleViewModel _viewModel;
	public CreateVehicleView(CreateVehicleViewModel vm)
	{
		InitializeComponent();
        _viewModel = vm;
		BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if(query != null)
        {
            if (query.TryGetValue("VehicleId", out var obj))
            {
                if (obj is int vehicleId)
                {
                    _viewModel.InitilizeUpdateVeicleCommand.Execute(vehicleId);
                }
            }
        }  
            _viewModel.InitilizeForCreateVeicleCommand.Execute(null);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();   
        _viewModel.InitilizeForCreateVeicleCommand.Execute(null);
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