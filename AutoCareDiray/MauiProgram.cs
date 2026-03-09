using AutoCareDiray.Shared.Data;
using AutoCareDiray.Shared.Service.Api;
using AutoCareDiray.Shared.Service.Data;
using AutoCareDiray.Service.Navigation;
using AutoCareDiray.Service.Dialog;
using AutoCareDiray.Shared.Models.Validation;
using AutoCareDiray.Shared.Service.ValidationService;
using AutoCareDiray.View.VehicleView;
using AutoCareDiray.Shared.ViewModels.VehicleViewModel;
using AutoCareDiray.Shared.ViewModels.RepairViewModel;
using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // Добавьте эту строку
using Microsoft.Extensions.Logging;
using AutoCareDiray.View.RepairView;
using AutoCareDiray.Shared.Interface;


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

            var DbPath = Path.Combine(FileSystem.AppDataDirectory, "vehiclesApp.db");
            builder.Services.AddDbContext<AppDBContex>(options =>
            {
                options.UseSqlite($"Data Source={DbPath}");
                #if Debug
                options.EnableSensitiveDataLogging();
                options.LogTo(Console.WriteLine, LogLevel.Information);
                #endif
            },ServiceLifetime.Scoped);
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
            builder.Services.AddSingleton<IDataService,DataService>();
            builder.Services.AddTransient<IDialogService,DialogService>();
            builder.Services.AddTransient<IValidatorService, ValidatorService>();
            builder.Services.AddSingleton<INavigationService,NavigationService>();

            //builder.Services.AddTransient<UserValidation>();
            //builder.Services.AddTransient<RecoverValidation>();
            builder.Services.AddTransient<VehicleValidation>();
            builder.Services.AddTransient<RepairValidation>();

            //builder.Services.AddTransient<AuthorizationPage>();
            //builder.Services.AddTransient<AuthorizationViewModel>();
            //builder.Services.AddTransient<RegistrationPage>();
            //builder.Services.AddTransient<RegistrationViewModel>();
            //builder.Services.AddTransient<RecoverPasswordView>();
            //builder.Services.AddTransient<RecoverPasswordViewModels>();
            //builder.Services.AddTransient<UserSettingsViewModal>();
            //builder.Services.AddTransient<UserSettingsPage>();

            builder.Services.AddTransient<ListVehicleView>();
            builder.Services.AddTransient<ListVehicleViewModel>();
            builder.Services.AddTransient<CardVehicleView>();
            builder.Services.AddTransient<CardVehicleViewModel>();
            builder.Services.AddTransient<CreateVehicleView>();
            builder.Services.AddTransient<CreateVehicleViewModel>();


            builder.Services.AddTransient<ListRepairView>();
            builder.Services.AddTransient<ListRepairViewModel>();
            builder.Services.AddTransient<CreateRepairView>();
            builder.Services.AddTransient<CreateRepairViewModel>();
            builder.Services.AddTransient<CardRepairView>();
            builder.Services.AddTransient<CardRepairViewModel>();



            // Регистрация сервиса

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
