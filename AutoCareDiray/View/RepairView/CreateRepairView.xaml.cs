using AutoCareDiray.Shared.ViewModels.RepairViewModel;
namespace AutoCareDiray.View.RepairView;

public partial class CreateRepairView : ContentPage, IQueryAttributable
{
    private readonly CreateRepairViewModel _viewModel;
	public CreateRepairView(CreateRepairViewModel vm)
	{
		InitializeComponent();
        _viewModel = vm;
		BindingContext = _viewModel;
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if(query.TryGetValue("VehicleId", out var obj))
        {
            if(obj is int vehicleId)
            {
                _viewModel.InitializeCommand.Execute(vehicleId);
            }
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
	protected override void OnDisappearing() 
	{ 
		base.OnDisappearing();
        if (BindingContext is CreateRepairViewModel vm)
        {
            vm.CancelTokenCommand.Execute(null);	
        }
    }
}