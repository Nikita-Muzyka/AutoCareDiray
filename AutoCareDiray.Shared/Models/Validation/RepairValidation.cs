using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Service.ResultService;
using AutoCareDiray.Shared.Service.ValidationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Models.Validation
{
    public class RepairValidation : ValidatorService
    {

        string propertyMileage = "MileageError";
        string propertyCost = "CostError";
        string propertyJob = "JobError";
        string propertyRepairType = "SelectedRepairTypeError";


        public RepairValidation() { }
        public bool ValidationMileage(int Mileage)
        {
            ErrorRemove(propertyMileage);
            if (Mileage >= 0)
            {
                OnErrorsChanges(propertyMileage);
                return false;
            }
            else
            {
                ErrorAdd(propertyMileage, "Пробег не может быть отрицательный");
                return true;
            }
        }
        public bool ValidationCost(decimal Cost)
        {
            ErrorRemove(propertyCost);
            if (Cost >= 0)
            {
                OnErrorsChanges(propertyCost);
                return false;
            }
            else
            {
                ErrorAdd(propertyCost, "Нельзя вводить орицательные числа");
                return true;
            }
        }
        public bool ValidationJob(string job)
        {
            ErrorRemove(propertyJob);
            if (string.IsNullOrEmpty(job) == false)
            {
                OnErrorsChanges(propertyJob);
                return false;
            }
            else
            {
                ErrorAdd(propertyJob, "Обязательно нужно выбрать где выполнялись работы");
                return true;
            }
        }
        public bool ValidationSelectedRepairType(RepairType type)
        {
            ErrorRemove(propertyRepairType);
            if (type != null )
            {
                OnErrorsChanges(propertyRepairType);
                return false;
            }
            else
            {
                ErrorAdd(propertyRepairType, "Обязательно нужно выбрать тип ремонта");
                return true;
            }
        }
    }
} 
