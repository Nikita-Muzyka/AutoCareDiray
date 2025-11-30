using AutoCareDiray.Service; // Ваши сервисы
using AutoCareDiray.View;
using AutoCareDiray.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection; // Добавьте эту строку
using Microsoft.Extensions.Logging;


namespace AutoCareDiray
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                 .UseMauiCommunityToolkit() // ← Добавьте эту строку!
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("Rubik-Regular.ttf", "RubikRegular");
                    fonts.AddFont("Rubik-Bold.ttf", "RubikBold");
                });


#if ANDROID && DEBUG
            string baseAddress = "http://192.168.0.105:5286/";
            TimeSpan time = TimeSpan.FromSeconds(500);
#elif DEBUG
            string baseAddress = "http://localhost:5286/";
            TimeSpan time = TimeSpan.FromSeconds(500);
#else
        string baseAddress = "2";
        TimeSpan.FromSeconds(30);
#endif

            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri(baseAddress),
                Timeout = time
            });


            builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddScoped<IDialogService,DialogService>();
            builder.Services.AddTransient<AuthorizationPage>();
            builder.Services.AddTransient<AuthorizationViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModal>();
            builder.Services.AddTransient<UserSettingsViewModal>();
            builder.Services.AddTransient<UserSettingsPage>();

            // Регистрация сервиса

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
