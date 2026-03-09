//using AutoCareDiray.Shared.Service.ValidationService;
//using AutoCareDiray.Shared.Interface;

//namespace AutoCareDiray.Shared.Models.Validation
//{
//    public class RecoverValidation : IValidatorService
//    {
//        public RecoverValidation(IApiService apiService) : base(apiService) { }

//        public string propertyLogin = "LoginError";
//        public string propertyNewPassword = "NewPasswordError";
//        public string propertyOldPassword = "OldPasswordError";

//        public void AllValidation(string newPassword,string oldPassword)
//        {
//            ValidationNewPassword(newPassword);
//            ValidationOldPassword(oldPassword);
//        }
//        public async Task ValidationLoginAsync(string login, CancellationToken token)
//        {
//            token.ThrowIfCancellationRequested();
//            ErrorRemove(propertyLogin);

//            if (!string.IsNullOrWhiteSpace(login))
//            {
//                if (login.Contains(" "))
//                {
//                    ErrorAdd(propertyLogin, "Логин - не должен содержать пробелы");
//                }
//                else
//                {
//                    OnErrorsChanges(propertyLogin);
//                }
//            }
//            else ErrorAdd(propertyLogin, "Логин - Обязателен к заполнению ");
//        }

//        public void ValidationNewPassword(string password)
//        {
//            ErrorRemove(propertyNewPassword);

//            if (!string.IsNullOrWhiteSpace(password))
//            {
//                if (password.Contains(" "))
//                {
//                    ErrorAdd(propertyNewPassword, "Пароль - не должен содержать пробелы");
//                }
//                else
//                {
//                    if (password.Length < 30 && password.Length > 6)
//                    {
//                        if (password.Any(char.IsNumber) &&
//                            password.Any(char.IsLetter) &&
//                            password.Any(char.IsUpper)
//                            )
//                        {
//                            OnErrorsChanges(propertyNewPassword);
//                        }
//                        else ErrorAdd(propertyNewPassword, "Пароль - Должен иметь Одну заглавную букву,одну цифру");
//                    }
//                    else ErrorAdd(propertyNewPassword, "Пароль - должен содержать не больше 20 символов и не меньше 6");
//                }
//            }
//            else ErrorAdd(propertyNewPassword, "Пароль - Обязателен к заполнению ");
//        }

//        public void ValidationOldPassword(string password)
//        {
//            ErrorRemove(propertyOldPassword);

//            if (!string.IsNullOrWhiteSpace(password))
//            {
//                if (password.Contains(" "))
//                {
//                    ErrorAdd(propertyOldPassword, "Пароль - не должен содержать пробелы");
//                }
//                else
//                {
//                    OnErrorsChanges(propertyOldPassword);
//                }
//            }
//            else ErrorAdd(propertyOldPassword, "Пароль - Обязателен к заполнению ");
//        }
//    }
//}
