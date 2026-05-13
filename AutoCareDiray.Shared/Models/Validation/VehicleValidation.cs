
using AutoCareDiray.Shared.Service.ValidationService;

namespace AutoCareDiray.Shared.Models.Validation
{
    public class VehicleValidation : ValidatorService
    {
        string propertyMileage = "MileageError";
        string propertyYearPurchase = "YearPurchaseError";
        string propertyTypeVehicle = "TypeVehicleError";
        string propertyNameVehicle = "NameVehicleError";



        public VehicleValidation() { }

        public void ValidationAll(string Mileage, DateTime yearPurchase,string typeVehicle,string nameVehicle)
        {
            ValidationMileage(Mileage);
            ValidationDate(yearPurchase);
            ValidationTypeVehicle(typeVehicle);
            ValidationNameVehicle(nameVehicle);
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
        public void ValidationDate(DateTime yearPurchase)
        {
            ErrorRemove(propertyYearPurchase);
            if (yearPurchase > new DateTime(1970,1,1))
            {
                OnErrorsChanges(propertyYearPurchase);
            }
            else
            {
                ErrorAdd(propertyYearPurchase, "Дата покупки обязательна");
            }

        }
        public void ValidationTypeVehicle(string typeVehicle)
        {
            ErrorRemove(propertyTypeVehicle);
            if (String.IsNullOrWhiteSpace(typeVehicle)) ErrorAdd(propertyTypeVehicle, "Обязательно нужно выбрать тип ТС");
            else OnErrorsChanges(propertyTypeVehicle);
        }
        public void ValidationNameVehicle(string nameVehicle)
        {
            ErrorRemove(propertyNameVehicle);
            if (String.IsNullOrWhiteSpace(nameVehicle)) ErrorAdd(propertyNameVehicle, "Обязательно нужно ввести имя");
            else OnErrorsChanges(propertyNameVehicle);
        }



    }
}
