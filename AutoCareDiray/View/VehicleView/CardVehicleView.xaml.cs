using AutoCareDiray.Shared.ViewModels.VehicleViewModel;

namespace AutoCareDiray.View.VehicleView;

public partial class CardVehicleView : ContentPage,IQueryAttributable
{
	private CardVehicleViewModel _viewModel;
	public CardVehicleView(CardVehicleViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = _viewModel;
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if(query.TryGetValue("VehicleId",out var obj))
		{
			if(obj is int vehicleId)
			{
				_viewModel.InitilizeCommand.Execute(vehicleId);
			}
		}
	}
}