
using AutoCareDiray.Shared.Service.ValidationService;

namespace AutoCareDiray.Shared.Models.Validation
{
    public class VehicleValidation : ValidatorService
    {
        string propertyMileage = "MileageError";
        string propertyYearPurchase = "YearPurchaseError";
        string propertyTypeVehicle = "TypeVehicleError";
        string propertyNameVehicle = "NameVehicleError";
        string propertyFuelTank = "FuelTankError";



        public VehicleValidation() { }

        public bool ValidationMileage(int Mileage)
        {
            ErrorRemove(propertyMileage);
            if (Mileage > 0)
            {
                OnErrorsChanges(propertyMileage);
                return false;
            }
            else
            {
                ErrorAdd(propertyMileage, "Нельзя вводить орицательные числа и 0");
                return true;
            }
        }
        public bool ValidationDate(DateTime yearPurchase)
        {
            ErrorRemove(propertyYearPurchase);
            if (yearPurchase > new DateTime(1970,1,1))
            {
                OnErrorsChanges(propertyYearPurchase);
                return false;
            }
            else
            {
                ErrorAdd(propertyYearPurchase, "Дата покупки обязательна");
                return true;
            }

        }
        public bool ValidationTypeVehicle(string typeVehicle)
        {
            ErrorRemove(propertyTypeVehicle);
            if (String.IsNullOrWhiteSpace(typeVehicle))
            {
                ErrorAdd(propertyTypeVehicle, "Обязательно нужно выбрать тип ТС");
                return true;
            }
            else
            {
                OnErrorsChanges(propertyTypeVehicle);
                return true;
            }
        }
        public bool ValidationNameVehicle(string nameVehicle)
        {
            ErrorRemove(propertyNameVehicle);
            if (String.IsNullOrWhiteSpace(nameVehicle))
            {
                ErrorAdd(propertyNameVehicle, "Обязательно нужно ввести имя");
                return true;
            }
            else
            {
                OnErrorsChanges(propertyNameVehicle);
                return false;
            }
        }
        public bool ValidationFuelTank(double fuelTank)
        {
            ErrorRemove(propertyFuelTank);
            if (fuelTank <= 0)
            {
                ErrorAdd(propertyFuelTank, "Нельзя вводить отрицательные числа или ноль");
                return true;
            }
            else
            {
                OnErrorsChanges(propertyFuelTank);
                return false;
            }
        }



    }
}
