using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Models
{
    public class UpdateUserPassword : UserRequest
    {
        public string oldPassword { get; set; }
        public string newPassword {  get; set; }
        public string Login { get; set; }

        public UpdateUserPassword(string login, string oldpassword,string newpassword) : base(login)
        {
            newPassword = oldpassword;
            oldpassword = newpassword;
        }
    }
}
