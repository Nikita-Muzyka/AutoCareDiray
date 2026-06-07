using AutoCareDiray.Extensions.SettingsEx;
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

    }
}
