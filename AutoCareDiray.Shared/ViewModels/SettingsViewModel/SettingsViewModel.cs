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
            SelectedDistanceUnit = _preferencesService.GetDefaultShortDistance();
            SelectedVolumeUnit = _preferencesService.GetDefaultShortVolume();
            SelectedMoneyUnit = _preferencesService.GetDefaultMoney();
        }

        // Срабатывает автоматически при выборе RadioButton расстояния
        partial void OnSelectedDistanceUnitChanged(string value)
        {
            if(int.TryParse(value, out var distanceUnit)) 
            {
                EUnitDistance setUnit = (EUnitDistance)distanceUnit;
                _preferencesService.SetUnitDistanse(setUnit);
            }
        }

        // Срабатывает автоматически при выборе RadioButton объема
        partial void OnSelectedVolumeUnitChanged(string value)
        {
            if (int.TryParse(value, out var volumeUnit))
            {
                EUnitVolume setUnit = (EUnitVolume)volumeUnit;
                _preferencesService.SetUnitVolume(setUnit);
            }
        }

        partial void OnSelectedMoneyUnitChanged(string value)
        {
            _preferencesService.SetDefault("UnitMoney", value);
        }
    }
}
