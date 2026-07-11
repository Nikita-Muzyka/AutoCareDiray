using AutoCareDiray.Shared.Extensions.UnitEx;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.SettignsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service.UnitService
{
    public class UnitService : IUnitService
    {

        const double ConvertedDistance = 1.60934;
        const double ConvertedVolume = 3.78541;
        public List<string> GetListUnitDistances()
        {
            List<string> listUnitDistances = new List<string>();
            foreach (EUnitDistance item in Enum.GetValues(typeof(EUnitDistance)))
            {
                string text = item.GetShorNameUnit();
                listUnitDistances.Add(text);
            }
             return listUnitDistances;
        }
        public List<string> GetListUnitVolume()
        {
            List<string> listUnitVolume = new List<string>();
            foreach (EUnitVolume item in Enum.GetValues(typeof(EUnitVolume)))
            {
                string text = item.GetShorNameUnit();
                listUnitVolume.Add(text);
            }
            return listUnitVolume;
        }

        public double GetConvertedMileage(double mileage, string unitDistance,string currentDistance)
        {
            if(unitDistance == EUnitDistance.Kilometers.GetShorNameUnit() && currentDistance != unitDistance)
            {
                var newMileage = mileage * ConvertedDistance;
                return mileage = (int)newMileage;
            }
            else if(unitDistance == EUnitDistance.Millie.GetShorNameUnit() && currentDistance != unitDistance)
            {
                var newMileage = mileage / ConvertedDistance;
               return mileage = newMileage;
            }

            return mileage;
        }
        public double GetConvertedVolume(double volume, string unitVolume, string currentVolume)
        {
            if (unitVolume == EUnitVolume.Liters.GetShorNameUnit() && currentVolume != unitVolume)
            {
                var newMileage = volume * ConvertedVolume;
                return volume = (int)newMileage;
            }
            else if (unitVolume == EUnitVolume.Gallon.GetShorNameUnit() && currentVolume != unitVolume)
            {
                var newMileage = volume / ConvertedVolume;
                return volume = newMileage;
            }

            return volume;
        }
    }
}
