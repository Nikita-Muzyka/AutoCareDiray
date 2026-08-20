
using AutoCareDiray.Shared.Extensions.UnitEx;
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
        private readonly IUnitService _unitService;

        public PreferencesService(IUnitService unitService)
        {
            _unitService = unitService;
        }

        public int GetDefault(string name)
        {
            var defUnit = Preferences.Default.Get(name, "0");
            return int.Parse(defUnit);
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

        public int GetUnitDistanseNumber()
        {
            return Preferences.Default.Get("DistanceUnit", 0);
        }
        public int GetUnitVolumeNumber()
        {
            return Preferences.Default.Get("VolumeUnit", 0);
        }

        public string GetDefaultShortDistance()
        {
            var unitResult = Preferences.Default.Get("DistanceUnit", 0);
                EUnitDistance unit = (EUnitDistance)unitResult;
            var result = unit.GetShorNameUnit();
            return result;
        }
        public string GetDefaultShortVolume()
        {
            var unitResult = Preferences.Default.Get("VolumeUnit", 0);
            EUnitVolume unit = (EUnitVolume)unitResult;
            var result = unit.GetShorNameUnit();
            return result;

        }
        public string GetDefaultMoney()
        {
            return Preferences.Default.Get("UnitMoney", "RUB");
        }
        public string GetDefaultMoneySign()
        {
            var money = Preferences.Default.Get("UnitMoney", "RUB");
            return CurrencyList.GetMoneySign(money);
        }
    }
}
