using AutoCareDiray.Shared.Models.RepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Interface
{
    public interface IRepairTypeCategoryService
    {
        string GetTitleCategory(RepairCategory category);
        RepairCategory GetCategory(string category);

        string GetIconCategory(RepairCategory category);
    }
}
