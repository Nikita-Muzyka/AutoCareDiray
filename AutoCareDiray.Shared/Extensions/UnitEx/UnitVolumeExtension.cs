using AutoCareDiray.Shared.Models.SettignsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Extensions.UnitEx
{
    public static class UnitVolumeExtension
    {
        public static string GetShorNameUnit(this EUnitVolume unit)
        {
            return unit switch
            {
                EUnitVolume.Liters => "Л",
                EUnitVolume.Gallon => "Гл"
            };
        }

        public static string GetNameUnit(this EUnitVolume unit)
        {
            return unit switch
            {
                EUnitVolume.Liters => "Литры",
                EUnitVolume.Gallon => "Галлоны"
            };
        }
    }
}
