using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
{
    class UserAuthorization : UserRequest
    {
        public string Password { get; set; }

        public UserAuthorization(string login,string password) : base(login)
        {
            Password = password;
        }
    }
}
