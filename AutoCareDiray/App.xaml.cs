using AutoCareDiray.Resources.Styles;
using AutoCareDiray.View;
namespace AutoCareDiray
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Light;
        }
        

        //public void SetTheme(AppTheme theme)
        //{
        //    var themeDict = (ResourceDictionary)Application.Current.Resources["ThemeDictionary"];
        //    themeDict.MergedDictionaries.Clear();


        //    if (theme == AppTheme.Dark)
        //    {
        //        Resources.MergedDictionaries.Add(new Resources.Styles.ColorsCustomDark());
        //    }
        //    else
        //    {
        //        Resources.MergedDictionaries.Add(new Resources.Styles.ColorsCustomLight());
        //    }

        //    // Принудительно устанавливаем тему MAUI
        //    Application.Current.UserAppTheme = theme;
        //}

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            window.Created += async (s, e) =>
            {
               
                bool check = Preferences.Get("is_login", false);
                if (check)
                {
                    await Shell.Current.GoToAsync("//Main");
                }
                else
                {
                    await Shell.Current.GoToAsync("//AuthorizationPage");
                }
            };
            
            return window;
        }
    }
}