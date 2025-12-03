using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models
{
    public class AuthorizationValidation
    {
        public static bool AuthValidation(string login,string password)
        {
            if(string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password)) return false;
            return true;
        }
    }
}
