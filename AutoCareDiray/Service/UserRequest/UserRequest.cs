using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
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
