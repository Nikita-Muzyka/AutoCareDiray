using AutoCareDiray.Service;
using AutoCareDiray.ViewModels;
namespace AutoCareDiray.View;

public partial class AuthorizationPage : ContentPage
{
    public AuthorizationPage(AuthorizationViewModel authViewModel)
    {
        InitializeComponent();
        BindingContext = authViewModel;
    }
}