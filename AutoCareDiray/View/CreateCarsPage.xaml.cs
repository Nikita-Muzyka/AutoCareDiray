using AutoCareDiray.ViewModels;
namespace AutoCareDiray.View;

public partial class CreateCarsPage : ContentPage
{
	public CreateCarsPage(CreateCarsViewModal CreateCarsVM)
	{
		InitializeComponent();
		BindingContext = CreateCarsVM;
	}
}