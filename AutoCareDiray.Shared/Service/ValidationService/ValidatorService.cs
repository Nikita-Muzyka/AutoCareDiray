using AutoCareDiray.Shared.Interface;
using System.Collections;
using System.ComponentModel;


namespace AutoCareDiray.Shared.Service.ValidationService
{
    public class ValidatorService : IValidatorService,INotifyDataErrorInfo
    {


        public ValidatorService()
        {

        }

        public bool HasErrors => _errors.Any();

        Dictionary<string, List<string>> _errors = new();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        //Возвращает ошибку нужному свойству
        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return _errors.Values.SelectMany(errors => errors);
            return _errors.ContainsKey(propertyName) ? _errors[propertyName].FirstOrDefault() : Enumerable.Empty<string>();
        }

        public bool GetVisibleErrors(string propertyName)
        {
            return false;
        }

        public void ErrorAdd(string propertyName,string value)
        {
            _errors.Add(propertyName, new List<string> { value });
            OnErrorsChanges(propertyName);
        }
        public void ErrorRemove(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
            }
        }
        public void OnErrorsChanges(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
