using AutoCareDiray.Shared.Models.SettignsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Interface
{
     public interface  IUnitService
    {
        List<string> GetListUnitDistances();
        List<string> GetListUnitVolume();
        double GetConvertedMileage(double mileage, string unitDistance, string currentDistance);
        double GetConvertedVolume(double volume, string unitVolume, string currentVolume);

    }
}
