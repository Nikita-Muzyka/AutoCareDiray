
using AutoCareDiray.Extensions.SettingsEx;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.SettignsModel;
using Microsoft.Maui.Storage;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service.PreferencesService
{
    public class PreferencesService : IPreferencesService
    {
        public string GetDefault(string name)
        {
            return Preferences.Default.Get(name, "0");
        }
        public void SetDefault(string name, string unit)
        {
            Preferences.Default.Set(name, unit);
        }

        public void SetUnitDistanse(EUnitDistance unit)
        {
            var unitDist = (int)unit;
            Preferences.Default.Set("DistanceUnit", unitDist);
        }
        public void SetUnitVolume(EUnitVolume unit)
        {
            var unitDist = (int)unit;
            Preferences.Default.Set("VolumeUnit", unitDist);
        }

        public string GetDefaultShortDistance()
        {
            var unitResult = Preferences.Default.Get("DistanceUnit", 0);
                EUnitDistance unit = (EUnitDistance)unitResult;
                return unit.GetShorNameUnit();
        }
        public string GetDefaultShortVolume()
        {
            var unitResult = Preferences.Default.Get("VolumeUnit", 0);
            EUnitVolume unit = (EUnitVolume)unitResult;
            return unit.GetShorNameUnit();

        }
        public string GetDefaultMoney()
        {
            return Preferences.Default.Get("MoneyUnit", "RUB");
        }
    }
}
