using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.SettignsModel;

namespace AutoCareDiray.Shared.ViewModels.SettingsViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly IPreferencesService _preferencesService;

        [ObservableProperty]
        private string selectedDistanceUnit;

        [ObservableProperty]
        private string selectedVolumeUnit;

        [ObservableProperty]
        private string selectedMoneyUnit;

        public SettingsViewModel(IPreferencesService preferences)
        {
            _preferencesService = preferences;

            // Просто вызываем наш метод при создании ViewModel
            InitializeAsync();
        }

        // async void отлично подходит для "fire-and-forget" инициализации из конструктора
        private async void InitializeAsync()
        {
            await Task.Delay(50); // Ждем отрисовку UI

            // ОБЯЗАТЕЛЬНО добавляем .ToString(), так как свойства ждут строку
            SelectedDistanceUnit = _preferencesService.GetUnitDistanseNumber().ToString();
            SelectedVolumeUnit = _preferencesService.GetUnitVolumeNumber().ToString();
            SelectedMoneyUnit = _preferencesService.GetDefaultMoney();
        }

        partial void OnSelectedDistanceUnitChanged(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (int.TryParse(value, out var distanceUnit))
            {
                _preferencesService.SetUnitDistanse((EUnitDistance)distanceUnit);
            }
        }

        partial void OnSelectedVolumeUnitChanged(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (int.TryParse(value, out var volumeUnit))
            {
                _preferencesService.SetUnitVolume((EUnitVolume)volumeUnit);
            }
        }

        partial void OnSelectedMoneyUnitChanged(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            _preferencesService.SetDefault("UnitMoney", value);
        }
    }
}