using AutoCareDiray.Models;
using AutoCareDiray.Models.Validation;
using AutoCareDiray.Service; // Ваши сервисы
using AutoCareDiray.Service.ValidationService;
using AutoCareDiray.View;
using AutoCareDiray.View.Maintenanse;
using AutoCareDiray.ViewModels;
using AutoCareDiray.ViewModels.Cars;
using AutoCareDiray.ViewModels.Maintenanse;
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


            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID

        handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS
                // Убираем рамку на iOS
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
        // Убираем рамку на Windows
        handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });


#if DEBUG
            string baseAddress = "http://localhost:5286/";
            TimeSpan time = TimeSpan.FromSeconds(30);
#endif

            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri(baseAddress),
                Timeout = time
            });

            builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddTransient<IDialogService,DialogService>();
            builder.Services.AddTransient<IValidatorService, ValidatorService>();
            builder.Services.AddTransient<UserValidation>();

            builder.Services.AddTransient<AuthorizationPage>();
            builder.Services.AddTransient<AuthorizationViewModel>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModal>();
            builder.Services.AddTransient<UserSettingsViewModal>();
            builder.Services.AddTransient<UserSettingsPage>();
            builder.Services.AddTransient<ListCars>();
            builder.Services.AddTransient<ListCarsViewModal>();
            builder.Services.AddTransient<CarCardPage>();
            builder.Services.AddTransient<CarCardViewModal>();
            builder.Services.AddTransient<CreateCarsPage>();
            builder.Services.AddTransient<CreateCarsViewModal>();
            builder.Services.AddTransient<CreateMaintenanse>();
            builder.Services.AddTransient<CreateMaintenanseViewModal>();
            builder.Services.AddTransient<ListMaintenanseView>();
            builder.Services.AddTransient<ListMaintenanseViewModel>();

            // Регистрация сервиса

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
