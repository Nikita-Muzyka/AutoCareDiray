
using AutoCareDiray.Shared.Service.ValidationService;

namespace AutoCareDiray.Shared.Models.Validation
{
    public class VehicleValidation : ValidatorService
    {
        string propertyMileage = "MileageError";
        string propertyYearPurchase = "YearPurchaseError";
        string propertyVehicleType = "VehicpeTypeError";


        public VehicleValidation() { }

        public void ValidationAll(string Mileage, DateTime? yearPurchase, DateTime? yearCreate)
        {
            ValidationMileage(Mileage);
            ValidationDate(yearPurchase,yearCreate);
        }
        public void ValidationMileage(string Mileage)
        {
            ErrorRemove(propertyMileage);
            if (string.IsNullOrWhiteSpace(Mileage) == false)
            {
                if (Mileage.Any(char.IsNumber) == true && Mileage.Any(char.IsLetter) == false)
                {
                    if(int.TryParse(Mileage,out int result))
                    {
                        if(result > 0)
                        {
                            OnErrorsChanges(propertyMileage);
                        }
                        else ErrorAdd(propertyMileage, "Нельзя вводить орицательные числа и 0");
                    }
                }
                else ErrorAdd(propertyMileage, "Поле должно содержать только цифры");
            }

            else ErrorAdd(propertyMileage, "Поле обязательно к заполнению");
        }

        public void ValidationDate(DateTime? yearPurchase, DateTime? yearCreate)
        {
            ErrorRemove(propertyYearPurchase);
            if (yearPurchase < yearCreate)
            {
                ErrorAdd(propertyYearPurchase, "Дата покупки не может быть раньше чем производство");
            }
            else
            {
                OnErrorsChanges(propertyYearPurchase);
            }
        }
    }
}
