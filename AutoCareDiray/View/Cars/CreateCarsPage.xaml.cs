using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Threading.Tasks;
namespace AutoCareDiray.View;

public partial class CreateCarsPage : ContentPage
{
	public CreateCarsPage(CreateCarsViewModal createCar)
	{
		InitializeComponent();
		BindingContext = createCar;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if(BindingContext is CreateCarsViewModal createCar)
        {
            createCar.CancelToken();
        }
    }
}