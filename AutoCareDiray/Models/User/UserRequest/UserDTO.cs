using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models.User
{
    public class UserDTO : UserRequest
    {
        public string NickName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }


        public UserDTO(string nickName, string email, string login, string password) : base(login)
        {
            NickName = nickName;
            Email = email;
            Password = password;
        }
    }
}
