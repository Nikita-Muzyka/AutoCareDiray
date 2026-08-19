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
        string GetDefault(string defailtName);
        void SetDefault(string defailtName, string unit);
        void SetUnitDistanse(EUnitDistance unit);
        void SetUnitVolume(EUnitVolume unit);
        string GetDefaultShortDistance();
        string GetDefaultShortVolume();
        string GetDefaultMoney();
        string GetDefaultMoneySign();

    }
}
