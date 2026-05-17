
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
        public string GetDefault(string defailtName,string unit)
        {
            return Preferences.Default.Get(defailtName, unit);
        }
        public void SetDefault(string defailtName, string unit)
        {
            Preferences.Default.Set(defailtName, unit);
        }
    }
}
