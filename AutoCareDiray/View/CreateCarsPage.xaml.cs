using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;
namespace AutoCareDiray.View;

public partial class CreateCarsPage : ContentPage
{
	public CreateCarsPage(IApiService apiService)
	{
		InitializeComponent();
		BindingContext = new CreateCarsViewModal(apiService);
	}
}