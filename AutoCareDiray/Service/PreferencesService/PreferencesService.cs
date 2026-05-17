
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Shared.Interface;

namespace AutoCareDiray.Service.PreferencesService
{
    public class PreferencesService : IPreferencesService
    {
        public string GetDefault(string defailtName)
        {
            return Preferences.Default.Get(defailtName, "non");
        }
        public void SetDefault(string defailtName, string unit)
        {
            Preferences.Default.Set(defailtName, unit);
        }

        public string GetDefaultDistance()
        {
            return Preferences.Default.Get("DistanceUnit", "non");
        }
        public string GetDefaultVolume()
        {
            return Preferences.Default.Get("VolumeUnit", "non");
        }
    }
}
