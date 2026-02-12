using System;
using System.Collections;
using AutoCareDiray.Service.ValidationService;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Service;

namespace AutoCareDiray.Models.Validation
{
    public class VehicleValidation : ValidatorService
    {
        //string propertyVinCode = "VinCode";
        string propertyMileage = "MileageError";
      

        public VehicleValidation(IApiService apiService) :base(apiService) { }

        //public void ValidationAll(string Mileage)
        //{
        //    ValidationMileage(Mileage);
        //}

        //public void ValidationVinCode(string VinCode)
        //{
        //    ErrorsClear(propertyVinCode);
        //    if (string.IsNullOrWhiteSpace(VinCode) == false)
        //    {
        //        if (VinCode.Length == 17) OnErrorsChanged(propertyVinCode); 
        //        else ErrorsAdd(propertyVinCode, "Vin Code должен сожержать 17 знаков");
        //    }
        //    else ErrorsAdd(propertyVinCode, "Поле обязательно к заполнению");
        //}
        public void ValidationMileage(string Mileage)
        {
            ErrorRemove(propertyMileage);
            if (string.IsNullOrWhiteSpace(Mileage) == false)
            {
                if (Mileage.Any(char.IsNumber) == true && Mileage.Any(char.IsLetter) == false) OnErrorsChanges(propertyMileage);
                else ErrorAdd(propertyMileage, "Поле должно содержать только цифры");
            }

            else ErrorAdd(propertyMileage, "Поле обязательно к заполнению");
        }
    }
}
