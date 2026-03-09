
namespace AutoCareDiray.Shared.Models.Validation
{
    public class LightLogInValidator
    {
        public static bool AuthValidation(string login,string password)
        {
            if(string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password)) return false;
            return true;
        }
    }
}
