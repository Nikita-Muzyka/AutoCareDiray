using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Threading.Tasks;
namespace AutoCareDiray.View;

public partial class CreateCarsPage : ContentPage
{
	public CreateCarsPage(IApiService apiService)
	{
		InitializeComponent();
		BindingContext = new CreateCarsViewModal(apiService);
    }
}