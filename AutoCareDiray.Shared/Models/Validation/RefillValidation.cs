using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Shared.Service.ValidationService;

namespace AutoCareDiray.Shared.Models.Validation
{
    public class RefillValidation : ValidatorService
    {
        string propertyMileage = "MileageError";
        string propertyCost = "CostError";
        string propertyFuelTypes = "FuelTypesError";
        string propertyVolumeLiters = "VolumeLitersError";

        public RefillValidation() { }

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
        public bool ValidationFuelTypes(string fuelTypes)
        {
            ErrorRemove(propertyFuelTypes);
            if (string.IsNullOrWhiteSpace(fuelTypes) == false)
            {
                OnErrorsChanges(propertyFuelTypes);
                return false;
            }
           else
            {
                ErrorAdd(propertyFuelTypes, "Выберите тип топлива");
                return true;
            }
        }
        public bool ValidationVolumeLiters(double volumeLiters)
        {
            ErrorRemove(propertyVolumeLiters);
            if (volumeLiters > 0)
            {
                OnErrorsChanges(propertyVolumeLiters);
                return false;
            }
            else
            {
                ErrorAdd(propertyVolumeLiters, "Нельзя вводить орицательные числа или ноль");
                return true;
            }
        }
    }
}
