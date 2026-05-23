using AutoCareDiray.Shared.Models.RepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Extensions
{
    public static class RepairCategoryExtension
    {
        public static string GetDisplay(this RepairCategory category)
        {
            return category switch
            {
                RepairCategory.RegularMaintenance => "Регульрное ТО",
                RepairCategory.Engine => "Двигатель",
                RepairCategory.Transmission => "Коробка",
                RepairCategory.Body => "Кузов",
                RepairCategory.Salon => "Салон авто",
                RepairCategory.Suspension => "Подвеска",
                RepairCategory.Electical => "Электрика",
                _ => "non"
            };
        }
    }
}
