using AutoCareDiray.Shared.Models.RepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Extensions.StringEx
{
    public static class StringCategoryExtension
    {
        public static RepairCategory GetCategory(this string category)
        {
            return category switch
            {
                "Регульрное ТО" => RepairCategory.RegularMaintenance,
                "Двигатель" => RepairCategory.Engine,
                "Коробка" => RepairCategory.Transmission,
                "Кузов" => RepairCategory.Body ,
                "Салон авто" => RepairCategory.Salon,
                "Подвеска" => RepairCategory.Suspension,
                "Электрика" => RepairCategory.Electrical,
                "Все категории" => RepairCategory.AllCategory
            };
        }
    }
}
