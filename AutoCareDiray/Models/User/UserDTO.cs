using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models
{
    public class UserDTO
    {
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }


        public UserDTO() { }

        public UserDTO(string nickName, string email, string password, string login)
        {
            NickName = nickName;
            Email = email;
            Password = password;
            Login = login;
        }
    }
}
