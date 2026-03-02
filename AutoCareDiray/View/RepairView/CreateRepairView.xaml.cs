using AutoCareDiray.ViewModels.RepairViewModel;
namespace AutoCareDiray.View.RepairView;

public partial class CreateRepairView : ContentPage
{
	public CreateRepairView(CreateRepairViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		if(BindingContext is CreateRepairViewModel vm)
		{
			vm.LoadingCommand.Execute(null);
		}
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