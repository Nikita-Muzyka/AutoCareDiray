using AutoCareDiray.Service;
using AutoCareDiray.Service.ValidationService;
using AutoCareDiray.Shared.DTOs.UserDTO;
using System.Net.Mail;


namespace AutoCareDiray.Models.Validation
{
    public class UserValidation : ValidatorService
    {

        public UserValidation(IApiService apiService) : base(apiService) { }

        public string propertyNickName = "NickNameError";
        public string propertyEmail = "EmailError";
        public string propertyLogin = "LoginError";
        public string propertyPassword = "PasswordError";

        public void ValidationAll(string nickname, string email, string password)
        {
            ValidationNickName(nickname);
            ValidationEmail(email);
            ValidationPassword(password);

        }
        public void ValidationNickName(string nickname)
        {
            ErrorRemove(propertyNickName);

            if (!string.IsNullOrWhiteSpace(nickname))
            {
                if (nickname.Contains(" "))
                {
                    ErrorAdd(propertyNickName, "Имя - не должен содержать пробелы");
                }
                else
                {
                    if (nickname.Length < 20)
                    {
                        OnErrorsChanges(propertyNickName);
                    }
                    else ErrorAdd(propertyNickName, "Имя - должен содержать не больше 20 символов");
                }
            }
            else ErrorAdd(propertyNickName, "Имя - Обязателен к заполнению ");
        }
        public void ValidationEmail(string email)
        {
            ErrorRemove(propertyEmail);

            if (!string.IsNullOrWhiteSpace(email))
            {
                if(email.Contains(" "))
                {
                    ErrorAdd(propertyEmail, "Email - не должен содержать пробелы");
                }
                else
                {
                    if (email.Length < 40)
                    {
                        try
                        {
                            var emailValid = new MailAddress(email);
                            OnErrorsChanges(propertyEmail);
                        }
                        catch (FormatException)
                        {
                            ErrorAdd(propertyEmail, "Email - Не верный формат адреса");
                        }
                        catch (ArgumentException)
                        {

                        }
                    }
                    else ErrorAdd(propertyEmail, "Email - должен содержать не больше 40 символов");
                }
            }
            else OnErrorsChanges(propertyEmail);
        }
        public async Task ValidationLoginAsync(string login, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            ErrorRemove(propertyLogin);

            if (!string.IsNullOrWhiteSpace(login))
            {
                if(login.Contains(" "))
                {
                    ErrorAdd(propertyLogin, "Логин - не должен содержать пробелы");
                }
                else
                {
                    if (login.Length < 20 && login.Length >= 4)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            var response = await _apiService.CheckUserLoginAsync(new UserLoginRequest(login), token);
                            token.ThrowIfCancellationRequested();
                            if (response.Success == true)
                            {
                                OnErrorsChanges(propertyLogin);
                            }
                            else
                            {
                                ErrorAdd(propertyLogin, response.ErrorMessage);
                            }
                        }
                        catch (OperationCanceledException) { }
                        catch (HttpRequestException)
                        {
                            ErrorAdd(propertyLogin, "Не удалось установить подключение, повторите попытку позже");
                        }
                    }
                    else ErrorAdd(propertyLogin, "Логин - должен содержать не больше 20 символов и не меньше 4");
                }
            }
            else ErrorAdd(propertyLogin, "Логин - Обязателен к заполнению ");
        }
        public void ValidationPassword(string password)
        {
            ErrorRemove(propertyPassword);

            if (!string.IsNullOrWhiteSpace(password))
            {
                if(password.Contains(" "))
                {
                    ErrorAdd(propertyPassword, "Пароль - не должен содержать пробелы");
                }
                else
                {
                    if (password.Length < 30 && password.Length > 6)
                    {
                        if (password.Any(char.IsNumber) &&
                            password.Any(char.IsLetter) &&
                            password.Any(char.IsUpper)
                            )
                        {
                            OnErrorsChanges(propertyPassword);
                        }
                        else ErrorAdd(propertyPassword, "Пароль - Должен иметь Одну заглавную букву,одну цифру");
                    }
                    else ErrorAdd(propertyPassword, "Пароль - должен содержать не больше 20 символов и не меньше 6");
                }
            }
            else ErrorAdd(propertyPassword, "Пароль - Обязателен к заполнению ");
        }
    }
}
