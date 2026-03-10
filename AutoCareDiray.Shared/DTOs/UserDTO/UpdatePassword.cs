using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.DTOs.UserDTO
{
    public class UpdatePassword
    {
        public int User_id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public UpdatePassword(int user_id,string oldPass,string newPass)
        {
            User_id = user_id;
            OldPassword = oldPass;
            NewPassword = newPass;
        }

        public UpdatePassword()
        { }
    }
}
