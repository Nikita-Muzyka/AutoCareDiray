using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;
namespace AutoCareDiray.View;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage(IApiService apiService)
	{
		InitializeComponent();
		BindingContext = new RegistrationViewModel(apiService);
	}
}