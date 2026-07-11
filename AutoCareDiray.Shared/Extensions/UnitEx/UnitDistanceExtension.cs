using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Shared.Models.SettignsModel;

namespace AutoCareDiray.Shared.Extensions.UnitEx
{
    public static class UnitDistanceExtension
    {
        public static string GetShorNameUnit(this EUnitDistance unit)
        {
            return unit switch
            {
                EUnitDistance.Kilometers => "Км",
                EUnitDistance.Millie => "Мл"
            };
        }

        public static string GetNameUnit(this EUnitDistance unit)
        {
            return unit switch
            {
                EUnitDistance.Kilometers => "Километры",
                EUnitDistance.Millie => "Мили"
            };
        }
    }
}
