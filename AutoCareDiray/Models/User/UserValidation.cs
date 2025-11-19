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
        string propertyNickName = "NickName";
        string propertyEmail = "Email";
        string propertyLogin = "Login";
        string propertyPassword = "Password";
        public bool HasErrors => _errors.Any();
        bool INotifyDataErrorInfo.HasErrors => HasErrors;

        Dictionary<string, List<string>> _errors = new();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        //Возвращает ошибку нужному свойству
        public IEnumerable GetErrors(string? propertyName)
        {
            if(string.IsNullOrEmpty(propertyName)) return _errors.Values.SelectMany(errors => errors);
            return _errors.ContainsKey(propertyName) ? _errors[propertyName].FirstOrDefault() : Enumerable.Empty<string>();
        }
        public void ValidationAll(string nickname,string email,string login,string password)
        {
            ValidationNickName(nickname);
            ValidationEmail(email);
            ValidationLogin(login);
            ValidationPassword(password);
        }
        public void ValidationNickName(string nickname)
        {
            ErrorsClear(propertyNickName);

            if (!string.IsNullOrWhiteSpace(nickname))
            {
                if (nickname.Length < 20)
                {
                    OnErrorsChange(propertyNickName);
                }
                else ErrorsAdd(propertyNickName, "NickName - должен содержать не больше 20 символов");
            }
            else ErrorsAdd(propertyNickName, "NickName - Обязателен к заполнению ");
        }
        public void ValidationEmail(string email)
        {
            ErrorsClear(propertyEmail);

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (email.Length < 40)
                {
                    
                }
                else ErrorsAdd(propertyEmail, "Email - должен содержать не больше 20 символов");
            }
        }
        public void ValidationLogin(string login)
        {
            ErrorsClear(propertyLogin);

            if (!string.IsNullOrWhiteSpace(login))
            {
                if (login.Length < 20 && login.Length > 4)
                {
                    OnErrorsChange(propertyLogin);
                }
                else ErrorsAdd(propertyLogin, "Login - должен содержать не больше 20 символов и не меньше 4");
            }
            else ErrorsAdd(propertyLogin, "Login - Обязателен к заполнению ");
        }
        public void ValidationPassword(string password)
        {
            ErrorsClear(propertyPassword);

            if (!string.IsNullOrWhiteSpace(password))
            {
                if (password.Length < 30 && password.Length > 6)
                {
                    if (password.Any(char.IsNumber) && 
                        password.Any(char.IsLetter) && 
                        password.Any(char.IsUpper) && 
                        password.Any(char.IsSymbol)
                        )
                    {
                        OnErrorsChange(propertyPassword);
                    }
                    else ErrorsAdd(propertyPassword, "Password - Должен иметь Одну заглавную букву,одну цифру,один символ (+, $, ©, ^ и т. д.)");
                }
                else ErrorsAdd(propertyPassword, "Password - должен содержать не больше 20 символов и не меньше 6");
            }
            else ErrorsAdd(propertyPassword, "Password - Обязателен к заполнению ");
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
            OnErrorsChange(propertyName);
        }
    }
}
