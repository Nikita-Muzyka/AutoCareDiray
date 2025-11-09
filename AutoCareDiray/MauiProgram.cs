using AutoCareDiray.Service; // Ваши сервисы
using AutoCareDiray.View;
using AutoCareDiray.ViewModels;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5286/"),
                Timeout = TimeSpan.FromSeconds(30)
            });

            builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddTransient<AuthorizationPage>();
            builder.Services.AddTransient<AuthorizationViewModel>();
            builder.Services.AddTransient<CreateCarsViewModal>();
            builder.Services.AddTransient<CreateCarsPage>();

            // Регистрация сервиса

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
