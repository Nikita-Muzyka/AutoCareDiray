using AutoCareDiray.Shared.ViewModels.RepairViewModel;

namespace AutoCareDiray.View.RepairView;

public partial class CardRepairView : ContentPage,IQueryAttributable
{
	CardRepairViewModel _viewModel;
    public CardRepairView(CardRepairViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = _viewModel;
	}

	public void ApplyQueryAttributes(IDictionary<string,object> parametr)
	{
		if(parametr.TryGetValue("RepairId",out  var obj))
		{
			if(obj is int result)
			{
				_viewModel.InitilizeCommand.Execute(result);
			}
		}

	}
}