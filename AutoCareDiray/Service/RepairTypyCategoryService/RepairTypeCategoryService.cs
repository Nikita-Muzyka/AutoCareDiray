using AutoCareDiray.Extensions;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service.RepairTypyCategoryService
{
    public class RepairTypeCategoryService : IRepairTypeCategoryService
    {
       public string GetTitleCategory(RepairCategory category)
        {
            string title = category.GetDisplay();
            return title;
        }

        public RepairCategory GetCategory(string category)
        {
            return category switch
            {
                "Регульрное ТО" => RepairCategory.RegularMaintenance,
                "Двигатель" => RepairCategory.Engine,
                "Коробка" => RepairCategory.Transmission,
                "Кузов" => RepairCategory.Body,
                "Салон авто" => RepairCategory.Salon,
                "Подвеска" => RepairCategory.Suspension,
                "Электрика" => RepairCategory.Electrical,
                _ => throw new NotImplementedException()
            };
        }

        public string GetIconCategory(RepairCategory category)
        {
            var icon = category.GetIconCategory();
            return icon;
        }
    }
}
