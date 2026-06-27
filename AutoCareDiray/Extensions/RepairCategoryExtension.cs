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
                RepairCategory.Electrical => "Электрика",
                _ => "non"
            };
        }

        public static string GetIconCategory(this RepairCategory category)
        {
            return category switch
            {
                RepairCategory.RegularMaintenance => "to_category_icon.png",
                RepairCategory.Engine => "engine_caregory_icon.png",
                RepairCategory.Transmission => "gearbox_category_icon.png",
                RepairCategory.Body => "body_category.png",
                RepairCategory.Salon => "salon_category.png",
                RepairCategory.Suspension => "suspension_category.png",
                RepairCategory.Electrical => "electrical_category.png",
                _ => "engine_caregory_icon.png"
            };
        }
    }
}
