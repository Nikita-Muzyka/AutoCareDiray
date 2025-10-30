using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models.User
{
    public class UserValidation
    {
        public static UserDTO Validation(string NickName, string Email, string Login, string Password)
        {
            UserDTO userDTO = new UserDTO(NickName,Email,Login,Password);
            return userDTO; 
        }
    }
}
