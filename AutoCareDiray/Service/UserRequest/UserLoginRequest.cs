using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
{
    public class UserLoginRequest : UserRequest
    {
        public  UserLoginRequest(string login) :base(login) { }
    }
}
