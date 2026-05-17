using AutoCareDiray.Service.Dialog;
using AutoCareDiray.Service.Navigation;
using AutoCareDiray.Service.PhotoPicker;
using AutoCareDiray.Shared.Data;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.Validation;
using AutoCareDiray.Service.PDF;
using AutoCareDiray.Shared.Service.Data;
using AutoCareDiray.Shared.Service.ValidationService;
using AutoCareDiray.Shared.ViewModels.RepairViewModel;
using AutoCareDiray.Shared.ViewModels.VehicleViewModel;
using AutoCareDiray.Shared.ViewModels.JournalEventViewModel;
using AutoCareDiray.View.RepairView;
using AutoCareDiray.View.VehicleView;
using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // Добавьте эту строку
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using QuestPDF.Infrastructure;
using AutoCareDiray.View.JournalEventView;


namespace AutoCareDiray
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            #region builder + settings Nug

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

            QuestPDF.Settings.License = LicenseType.Community;

            #endregion

            #region Settings UI

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
             handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());   
#elif IOS
                // Убираем рамку на iOS
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
                // Убираем рамку на Windows
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });


            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
            {
#if ANDROID

        handler.PlatformView.Background = null;
        handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS

                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
        handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });


            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
                // Убираем подчеркивание у Editor на Android
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });

            #endregion

            #region Sqlite

            var DbPath = Path.Combine(FileSystem.AppDataDirectory, "auto_care_diray.db");
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

            builder.Services.AddScoped<HttpClient>(sp =>
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

            #endregion

            #region DI

            //builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddScoped<IDataService,DataService>();
            builder.Services.AddTransient<IDialogService,DialogService>();
            builder.Services.AddTransient<IValidatorService, ValidatorService>();
            builder.Services.AddSingleton<INavigationService,NavigationService>();
            builder.Services.AddTransient<IPhotoPicker,PhotoPicker>();
            builder.Services.AddTransient<IPdfService,PdfService>();
            builder.Services.AddTransient<IPreferencesService,AutoCareDiray.Service.PreferencesService.PreferencesService>();


            builder.Services.AddTransient<VehicleValidation>();
            builder.Services.AddTransient<RepairValidation>();
            builder.Services.AddTransient<RefillValidation>();



            builder.Services.AddTransient<ListVehicleView>();
            builder.Services.AddTransient<ListVehicleViewModel>();
            builder.Services.AddTransient<CardVehicleView>();
            builder.Services.AddTransient<CardVehicleViewModel>();
            builder.Services.AddTransient<CreateVehicleView>();
            builder.Services.AddTransient<CreateVehicleViewModel>();

            builder.Services.AddTransient<JournalEventView>();
            builder.Services.AddTransient<JournalEventViewModel>();

            builder.Services.AddTransient<AutoCareDiray.View.RefillView.CreateRefillView>();
            builder.Services.AddTransient<AutoCareDiray.Shared.ViewModels.RefillViewModel.CreateRefillViewModel>();
            builder.Services.AddTransient<AutoCareDiray.View.RefillView.CardRefillView>();
            builder.Services.AddTransient<AutoCareDiray.Shared.ViewModels.RefillViewModel.CardRefillViewModel>();

            builder.Services.AddTransient<AutoCareDiray.View.SettingsView.SettingsView>();
            builder.Services.AddTransient<AutoCareDiray.Shared.ViewModels.SettingsViewModel.SettingsViewModel>();


            builder.Services.AddTransient<CreateRepairView>();
            builder.Services.AddTransient<CreateRepairViewModel>();
            builder.Services.AddTransient<CardRepairView>();
            builder.Services.AddTransient<CardRepairViewModel>();

            #endregion

            #region debugsettings
#if DEBUG
            builder.Logging.AddDebug();
#endif
            #endregion

            return builder.Build();
        }
    }
}
