using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;
namespace AutoCareDiray.View;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage(RegistrationViewModel rvm)
	{
		InitializeComponent();
		BindingContext = rvm;
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
		if(BindingContext is RegistrationViewModel rvm)
		{
			rvm.CancelToken();
        }
    }
}