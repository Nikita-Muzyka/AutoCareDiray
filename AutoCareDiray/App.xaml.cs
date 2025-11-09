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
                // Показываем модальное окно авторизации
                //var authPage = Handler.MauiContext.Services.GetService<AuthorizationPage>();
                //await Shell.Current.Navigation.PushModalAsync(authPage);
                await Shell.Current.GoToAsync("//CreateCarsPage");
            };

            return window;
        }
    }
}