using AutoCareDiray.Resources.Styles;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.View;
using System.Diagnostics;
namespace AutoCareDiray
{
    public partial class App : Application
    {
        private readonly IDataService _dataService;
        public App(IDataService dataService)
        {
            _dataService = dataService;
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
                if(Preferences.Get("firstEntry", true))
                {
                    Preferences.Set("UnitDistance", "0");
                    Preferences.Set("UnitVolume", "0");
                    Preferences.Set("UnitMoney", "RUB");

                    Preferences.Set("firstEntry", false);
                }
                else
                {

                }
                _dataService.InitializeDatabase();
                await Shell.Current.GoToAsync("//ListVehicle");
            };
            
            return window;
        }
    }
}