using CommunityToolkit.Mvvm.ComponentModel;
using AutoCareDiray.Shared.Interface;


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
            SelectedDistanceUnit = _preferencesService.GetDefaultDistance();
            SelectedVolumeUnit = _preferencesService.GetDefaultVolume();
            SelectedMoneyUnit = _preferencesService.GetDefaultMoney();
        }

        // Срабатывает автоматически при выборе RadioButton расстояния
        partial void OnSelectedDistanceUnitChanged(string value)
        {
            _preferencesService.SetDefault("DistanceUnit", value);
        }

        // Срабатывает автоматически при выборе RadioButton объема
        partial void OnSelectedVolumeUnitChanged(string value)
        {
            _preferencesService.SetDefault("VolumeUnit", value);
        }

        partial void OnSelectedMoneyUnitChanged(string value)
        {
            _preferencesService.SetDefault("MoneyUnit", value);
        }
    }
}
