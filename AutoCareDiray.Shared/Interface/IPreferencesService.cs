using AutoCareDiray.Shared.Models.SettignsModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Interface
{
    public interface IPreferencesService
    {
        int GetDefault(string defailtName);
        void SetDefault(string defailtName, string unit);
        void SetUnitDistanse(EUnitDistance unit);
        void SetUnitVolume(EUnitVolume unit);
        int GetUnitDistanseNumber();
        int GetUnitVolumeNumber();
        string GetDefaultShortDistance();
        string GetDefaultShortVolume();
        string GetDefaultMoney();
        string GetDefaultMoneySign();

    }
}
