using AutoCareDiray.View;
namespace AutoCareDiray
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            window.Created += async (s, e) =>
            {
                await Shell.Current.GoToAsync("//MainPage");
                //bool check = Preferences.Get("is_login", false);
                //if (check)
                //{
                //    await Shell.Current.GoToAsync("//MainPage");
                //}
                //else
                //{
                //    var authPage = Handler.MauiContext.Services.GetService<AuthorizationPage>();
                //    await Shell.Current.Navigation.PushModalAsync(authPage);
                //}
            };
            
            return window;
        }
    }
}