using AutoCareDiray.ViewModels.RepairViewModel;

namespace AutoCareDiray.View.RepairView;

public partial class ListRepairView : ContentPage
{
	public ListRepairView(ListRepairViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		if(BindingContext is ListRepairViewModel vm)
		{
			vm.LoadDataCommand.Execute(null);
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
}