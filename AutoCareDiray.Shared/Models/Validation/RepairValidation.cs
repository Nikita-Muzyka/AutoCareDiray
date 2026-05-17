using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Service.ValidationService;

namespace AutoCareDiray.Shared.Models.Validation
{
    public class RepairValidation : ValidatorService
    {

        string propertyMileage = "MileageError";
        string propertyCost = "CostError";


        public RepairValidation() { }

        public void ValidationAll(int Mileage,string Cost)
        {
            ValidationMileage(Mileage);
            ValidationCost(Cost);
        }
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
        public bool ValidationCost(string Cost)
        {
            //ErrorRemove(propertyCost);
            //if (string.IsNullOrWhiteSpace(Cost) == false)
            //{
            //    if (Cost.Any(char.IsNumber) == true && Cost.Any(char.IsLetter) == false)
            //    {
            //        if (int.TryParse(Cost, out int result))
            //        {
            //            if (result >= 0)
            //            {
            //                OnErrorsChanges(propertyCost);
            //                return false;
            //            }
            //            else 
            //            {
            //                ErrorAdd(propertyCost, "Нельзя вводить орицательные числа");
            //                return true;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        ErrorAdd(propertyCost, "Поле должно содержать только цифры");
            //        return true;
            //    }
            //}
            //else OnErrorsChanges(propertyCost);
            return true;
        }
    }
}       
