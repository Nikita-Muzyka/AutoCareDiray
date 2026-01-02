using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models.User
{
    public abstract class UserRequest
    {
        public string Login { get; set; }
        public UserRequest(string login) 
        {
            Login = login;
        }
    }
}
