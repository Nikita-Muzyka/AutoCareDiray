using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models
{
    public class UserValidation : INotifyDataErrorInfo
    {
        string propertyLogin = "Login";
        string propertyNickName = "NickName";
        public bool HasErrors => _errors.Any();
        bool INotifyDataErrorInfo.HasErrors => HasErrors;

        Dictionary<string, List<string>> _errors = new();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if(string.IsNullOrEmpty(propertyName)) return _errors.Values.SelectMany(errors => errors);
            return _errors.ContainsKey(propertyName) ? _errors[propertyName].FirstOrDefault() : Enumerable.Empty<string>();
        }

        public void ValidationNickName(string nickname)
        {
            ErrorsClear(propertyLogin);

            if (!string.IsNullOrWhiteSpace(nickname))
            {
                if (nickname.Length < 20)
                {
                    OnErrorsChange(propertyLogin);
                }
                else ErrorsAdd(propertyLogin, "Login - должен содержать не больше 20 символов");
            }
            else ErrorsAdd(propertyLogin, "Login - Обязателен к заполнению ");
        }
        public void ValidationLogin(string login)
        {
            ErrorsClear(propertyLogin);

            if (!string.IsNullOrWhiteSpace(login))
            {
                if (login.Length < 20)
                {
                    OnErrorsChange(propertyLogin);
                }
                else ErrorsAdd(propertyLogin, "Login - должен содержать не больше 20 символов");
            }
            else ErrorsAdd(propertyLogin, "Login - Обязателен к заполнению ");
        }

        void OnErrorsChange(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        void ErrorsClear(string propertyName)
        {
            if (_errors.ContainsKey(propertyName)) 
            {
                _errors.Remove(propertyName);
            }
        }
        void ErrorsAdd(string propertyName,string value)
        {
            _errors.Add(propertyName,new List<string> {value});
            OnErrorsChange(propertyLogin);
        }
    }
}
