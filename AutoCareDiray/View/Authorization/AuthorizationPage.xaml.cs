namespace AutoCareDiray.View;

public partial class AuthorizationPage : ContentPage
{
    public AuthorizationPage(/*AuthorizationViewModel authViewModel*/)
    {
        InitializeComponent();
        //BindingContext = authViewModel;
    }
    //protected override void OnDisappearing()
    //{
    //    base.OnDisappearing();
    //    if(BindingContext is AuthorizationViewModel authViewModel)
    //    {
    //        authViewModel.CancelToken();
    //    }
    //}
}