using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service.ValidationService
{
    public class ValidatorService : IValidatorService,INotifyDataErrorInfo
    {
        protected readonly IApiService _apiService;

        public ValidatorService(IApiService apiService)
        {
            _apiService = apiService;
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
