using AutoCareDiray.Models;
using AutoCareDiray.Models.Validation;
using AutoCareDiray.Service; // Ваши сервисы
using AutoCareDiray.Service.ValidationService;
using AutoCareDiray.View;
using AutoCareDiray.View.Authorization;
using AutoCareDiray.View.Maintenanse;
using AutoCareDiray.View.VehicleView;
using AutoCareDiray.ViewModels;
using AutoCareDiray.ViewModels.Authrozation;
using AutoCareDiray.ViewModels.Maintenanse;
using AutoCareDiray.ViewModels.VehicleViewModel;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection; // Добавьте эту строку
using Microsoft.Extensions.Logging;
using UraniumUI;


namespace AutoCareDiray
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                 .UseMauiCommunityToolkit()
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


            string baseAddress = "http://localhost:5286/";
            TimeSpan time = TimeSpan.FromSeconds(30);

            builder.Services.AddSingleton<HttpClient>(sp =>
            {
                var handler = new HttpClientHandler
                {
                    UseProxy = false
                };

                var client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(baseAddress),
                    Timeout = new TimeSpan(time.Ticks)
                };

                return client;
            });

            builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddTransient<IDialogService,DialogService>();
            builder.Services.AddTransient<IValidatorService, ValidatorService>();

            builder.Services.AddTransient<UserValidation>();
            builder.Services.AddTransient<RecoverValidation>();
            builder.Services.AddTransient<VehicleValidation>();

            builder.Services.AddTransient<AuthorizationPage>();
            builder.Services.AddTransient<AuthorizationViewModel>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<RecoverPasswordView>();
            builder.Services.AddTransient<RecoverPasswordViewModels>();
            builder.Services.AddTransient<UserSettingsViewModal>();
            builder.Services.AddTransient<UserSettingsPage>();
            builder.Services.AddTransient<ListVehicleView>();
            builder.Services.AddTransient<ListVehicleViewModel>();
            builder.Services.AddTransient<CardVehicleView>();
            builder.Services.AddTransient<CardVehicleViewModel>();
            builder.Services.AddTransient<CreateVehicleView>();
            builder.Services.AddTransient<CreateVehicleViewModel>();
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
