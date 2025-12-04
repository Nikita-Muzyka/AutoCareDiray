using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service
{
    public class UserUpdateRequest : UserRequest
    {
        public int User_Id { get; set; }
        public string NickName { get;set; }
        public string Email { get; set; }
        public UserUpdateRequest(int user_id,string nickname,string email,string login) : base(login)
        {
            User_Id = user_id;
            NickName = nickname;
            Email = email;
        }
    }
}
