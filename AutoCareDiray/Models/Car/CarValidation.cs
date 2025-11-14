using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models.Car
{
    public class CarValidation : INotifyDataErrorInfo
    {
        string propertyVinCode = "VinCode";
        string propertyMileage = "Mileage";
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return _errors.Values.SelectMany(errors => errors);
            return _errors.ContainsKey(propertyName) ? _errors[propertyName].FirstOrDefault() : Enumerable.Empty<string>();
        }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public bool HasErrors => _errors.Any();

        public CarValidation() { }

        public void ValidationAll(string VinCode, string Mileage)
        {
            ValidationVinCode(VinCode);
            ValidationMileage(Mileage);
        }
        public void ValidationVinCode(string VinCode)
        {
            ErrorsClear(propertyVinCode);
            if (string.IsNullOrWhiteSpace(VinCode) == false)
            {
                if (VinCode.Length < 17) ;
                else ErrorsAdd(propertyVinCode, "Vin Code не должен содержать больше 17 знаков");
            }
            else ErrorsAdd(propertyVinCode, "Поле обязательно к заполнению");
        }
        public void ValidationMileage(string Mileage)
        {
            ErrorsClear(propertyMileage);
            if (string.IsNullOrWhiteSpace(Mileage) == false
                && Mileage.Any(char.IsNumber) == true
                && Mileage.Any(char.IsLetter) == true) ;

            else ErrorsAdd(propertyMileage, "Поле обязательно к заполнению");
        }
        void ErrorsClear(string PropertyName)
        {
            if(_errors.ContainsKey(PropertyName)) _errors.Remove(PropertyName);
        }
        void ErrorsAdd(string propertyName, string value)
        {
            _errors.Add(propertyName,new List<string> { value });
            OnErrorsChanged(propertyName);
        }
        void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
